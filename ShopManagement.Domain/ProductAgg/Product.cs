using System.Security.Permissions;
using _0_Framework.Domain;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Domain.ProductAgg;

public class Product : EntityBase
{
    public string Name { get; private set; }
    public string Code { get; private set; }
    public bool IsInStock { get; private set; }
    public double UnitPrice { get; private set; }
    public int Discount { get; private set; }
    public string ShortDescription { get; private set; }
    public string Description { get; private set; }
    public ProductCategory Category { get; private set; }
    public long CategoryId { get; private set; }
    public long Count { get; private set; }
    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }
    public string PictureTitle { get; private set; }
    public string Keywords { get; private set; }
    public string MetaDescription { get; private set; }
    public string Slug { get; private set; }
    public List<ProductPicture> ProductPictures { get; private set; }


    public Product(string name, double unitPrice, string? description, string? picture,
        string? pictureAlt, string? pictureTitle, string keywords, string metaDescription,
        string slug, string code, string shortDescription, long categoryId)
    {
        Name = name;
        Code = code;
        IsInStock = true;
        UnitPrice = unitPrice;
        Discount = 0;
        ShortDescription = shortDescription;
        Description = description;
        CategoryId = categoryId;
        Count = 1;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Keywords = keywords;
        MetaDescription = metaDescription;
        Slug = slug;
    }

    public void Edit(string name, double unitPrice, string? description, string? picture,
        string? pictureAlt, string? pictureTitle, string keywords, string metaDescription,
        string slug, string code, string shortDescription, long categoryId)
    {
        Name = name;
        Code = code;
        UnitPrice = UnitPrice;
        ShortDescription = shortDescription;
        Description = description;
        CategoryId = categoryId;
        Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Keywords = keywords;
        MetaDescription = metaDescription;
        Slug = slug;
    }

    public void SetDiscount(int discount)
    {
        Discount = discount;
    }

    public void AddToInventory(int amount)
    {
        Count += amount;
    }

    public void RemoveFromInventory(int amount)
    {
        Count -= amount;
    }

    public void Sell()
    {
        Count--;
    }

    public void InStock()
    {
        IsInStock = true;
    }

    public void NotInStock()
    {
        IsInStock = false;
    }
}