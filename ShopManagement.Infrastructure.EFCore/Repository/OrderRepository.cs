using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Infrastructure.EFCore;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts;
using ShopManagement.Application.Contracts.Order;
using ShopManagement.Domain.OrderAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository;

public class OrderRepository : RepositoryBase<long, Order>, IOrderRepository
{
    private readonly AccountContext _accountContext;
    private readonly ShopContext _shopContext;

    public OrderRepository(ShopContext shopContext,
        AccountContext accountContext) : base(shopContext)
    {
        _shopContext = shopContext;
        _accountContext = accountContext;
    }

    public double GetAmountBy(long id)
    {
        return _shopContext.Orders
            .Select(x => new { x.Id, x.PayAmount })
            .FirstOrDefault(x => x.Id == id)!.PayAmount;
    }

    public List<OrderViewModel> Search(OrderSearchModel searchModel)
    {
        var accounts = _accountContext.Accounts
            .Select(x => new { x.Id, x.FullName });

        var query = _shopContext.Orders
            .Select(x => new OrderViewModel
            {
                Id = x.Id,
                AccountId = x.AccountId,
                DiscountAmount = x.DiscountAmount,
                IsCanceled = x.IsCanceled,
                IsPaid = x.IsPaid,
                IssueTrackingNo = x.IssueTrackingNo,
                PayAmount = x.PayAmount,
                PaymentMethodId = x.PaymentMethod,
                RefId = x.RefId,
                TotalAmount = x.TotalAmount,
                CreationDate = x.CreationDate.ToFarsi()
            });

        query = query.Where(x => x.IsCanceled == searchModel.IsCanceled);

        if (searchModel.AccountId > 0)
            query = query.Where(x => x.AccountId == searchModel.AccountId);

        var orders = query.OrderByDescending
            (x => x.Id).ToList();

        orders.ForEach(order =>
        {
            order.AccountFullName = accounts.FirstOrDefault
                (a => a.Id == order.AccountId)!.FullName;

            order.PaymentMethod = PaymentMethod.GetBy(order.PaymentMethodId).Name;
        });

        return orders;
    }

    public List<OrderItemViewModel> GetItems(long orderId)
    {
        var products = _shopContext.Products
            .Select(x => new { x.Id, x.Name }).ToList();

        var order = _shopContext.Orders
            .Include(x => x.Items)
            .FirstOrDefault(x => x.Id == orderId);
        if (order == null)
            return [];

        var items = order.Items
            .Select(x => new OrderItemViewModel
            {
                Id = x.Id,
                OrderId = x.OrderId,
                Count = x.Count,
                DiscountRate = x.DiscountRate,
                ProductId = x.ProductId,
                UnitPrice = x.UnitPrice
            }).ToList();


        items.ForEach(item =>
            item.Product = products.FirstOrDefault
                (p => p.Id == item.ProductId)!.Name);

        return items;
    }
}