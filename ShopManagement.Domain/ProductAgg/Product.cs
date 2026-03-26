using _0_Framework.Domain;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Domain.ProductAgg;

public class Product : EntityBase
{
    public Product(string name, string? description, string? picture,
        string? pictureAlt, string? pictureTitle, string keywords, string metaDescription,
        string slug, string code, string shortDescription, long categoryId)
    {
        Name = name;
        Code = code;
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

    public string Name { get; private set; }
    public string Code { get; private set; }
    public string ShortDescription { get; private set; }
    public string Description { get; private set; }
    public long CategoryId { get; private set; }
    public string Picture { get; private set; }
    public string PictureAlt { get; private set; }
    public string PictureTitle { get; private set; }
    public string Keywords { get; private set; }
    public string MetaDescription { get; private set; }
    public string Slug { get; private set; }
    public List<ProductPicture> ProductPictures { get; private set; }
    public ProductCategory Category { get; private set; }

    public void Edit(string name, string? description, string? picture,
        string? pictureAlt, string? pictureTitle, string keywords, string metaDescription,
        string slug, string code, string shortDescription, long categoryId)
    {
        Name = name;
        Code = code;
        ShortDescription = shortDescription;
        Description = description;
        CategoryId = categoryId;
        if (string.IsNullOrEmpty(picture))
            Picture = picture;
        PictureAlt = pictureAlt;
        PictureTitle = pictureTitle;
        Keywords = keywords;
        MetaDescription = metaDescription;
        Slug = slug;
    }
}