using _0_Framework.Application;
using _0_Framework.Application.Sms;
using Microsoft.Extensions.Configuration;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;
using ShopManagement.Domain.Services;

namespace ShopManagement.Application;

public class OrderApplication : IOrderApplication
{
    private readonly IAuthHelper _authHelper;
    private readonly IConfiguration _configuration;
    private readonly IOrderRepository _orderRepository;
    private readonly IShopInventoryAcl _shopInventoryAcl;
    private readonly ISmsService _smsService;
    private readonly IShopAccountAcl _accountAcl;
    
    public OrderApplication(IOrderRepository orderRepository,
        IAuthHelper authHelper, IConfiguration configuration,
        IShopInventoryAcl shopInventoryAcl, ISmsService smsService, IShopAccountAcl accountAcl)
    {
        _orderRepository = orderRepository;
        _authHelper = authHelper;
        _configuration = configuration;
        _shopInventoryAcl = shopInventoryAcl;
        _smsService = smsService;
        _accountAcl = accountAcl;
    }

    public long PlaceOrder(Cart cart)
    {
        var currentAccountId = _authHelper.CurrentAccountId();

        var order = new Order(currentAccountId, cart.TotalAmount,
            cart.DiscountAmount, cart.PayAmount, cart.PaymentMethod);

        foreach (var orderItem in cart.Items.Select(item => new OrderItem(item.Id, item.Count,
                     item.UnitPrice, item.DiscountRate)))
            order.AddItem(orderItem);

        _orderRepository.Create(order);
        _orderRepository.SaveChanges();

        return order.Id;
    }

    public string PaymentSucceeded(long orderId, long refId)
    {
        var order = _orderRepository.Get(orderId);
        if (order == null)
            return "";

        order.PaymentSucceeded(refId);

        var symbol = _configuration["Symbol"];
        var issueTrackingNo = CodeGenerator.Generate(symbol!);
        order.SetIssueTrackingNo(issueTrackingNo);

        if (!_shopInventoryAcl.ReduceFromInventory(order.Items))
            return "";

        _orderRepository.SaveChanges();
        
        var (name, mobile) = _accountAcl.
            GetAccountBy(order.AccountId);

        _smsService.Send(mobile,
            $"{name} گرامی سفارش شما با شماره پیگیری" +
            $" {issueTrackingNo} با موفقیت پرداخت شد و ارسال خواهد شد.");
        
        return issueTrackingNo;
    }

    public void Cancel(long id)
    {
        _orderRepository.Get(id)!.Cancel();
        _orderRepository.SaveChanges();
    }

    public double GetAmountBy(long id)
    {
        return _orderRepository.GetAmountBy(id);
    }

    public List<OrderItemViewModel> GetItems(long orderId)
    {
        return _orderRepository.GetItems(orderId);
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        return _orderRepository.Search(searchModel);
    }
}