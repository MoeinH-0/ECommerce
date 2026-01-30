using System.ComponentModel.DataAnnotations;
using ShopManagement.Application.Contracts.Product;

namespace DiscountManagement.Application.Contract.ColleagueDiscount;

public class DefineColleagueDiscount
{
    [Range(1, 10000)] public long ProductId { get; set; }

    [Range(1, 100)] public int DiscountRate { get; set; }

    public List<ProductViewModel> Products { get; set; }
}