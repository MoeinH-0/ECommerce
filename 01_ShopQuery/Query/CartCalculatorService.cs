using _0_Framework.Application;
using _0_Framework.Infrastructure;
using _01_ShopQuery.Contracts.CartContract;
using DiscountManagement.Infrastructure.EFCore;
using ShopManagement.Application.Contracts.Order;

namespace _01_ShopQuery.Query;

public class CartCalculatorService : ICartCalculatorService
{
    private readonly DiscountContext _discountContext;
    private readonly IAuthHelper _authHelper;

    public CartCalculatorService(DiscountContext discountContext,
        IAuthHelper authHelper)
    {
        _discountContext = discountContext;
        _authHelper = authHelper;
    }

    public Cart ComputeCart(List<CartItem> cartItems)
    {
        var result = new Cart();
        var currentAccountRole = _authHelper.CurrentAccountRole();

        var discounts = currentAccountRole == Roles.ColleagueUser ?
                GetColleagueDiscounts() : GetCustomerDiscounts();

        foreach (var cartItem in cartItems)
        {
            var discount = discounts
                .FirstOrDefault(x => cartItem.Id == x.ProductId);

            if (discount != null)
                cartItem.DiscountRate = discount.DiscountRate;
    
            result.Add(cartItem);
        }

        return result;
    }

    private List<DiscountListViewModel> GetColleagueDiscounts()
    {
        return _discountContext.ColleagueDiscounts
            .Where(x => !x.IsRemved)
            .Select(x => new DiscountListViewModel
            {
                ProductId = x.ProductId,
                DiscountRate = x.DiscountRate
            })
            .ToList();
    }

    private List<DiscountListViewModel> GetCustomerDiscounts()
    {
        return _discountContext.CustomerDiscounts
            .Where(x => DateTime.UtcNow >= x.StartDate
                        && DateTime.UtcNow <= x.EndDate)
            .Select(x => new DiscountListViewModel
            {
                ProductId = x.ProductId,
                DiscountRate = x.DiscountRate
            })
            .ToList();
    }
}