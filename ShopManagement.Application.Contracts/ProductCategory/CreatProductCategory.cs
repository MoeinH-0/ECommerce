namespace ShopManagement.Application.Contracts.ProductCategory;

public class CreatProductCategory
{
    public required string Name { get; set; }

    public required string Description { get; set; }

    public required string Picture { get; set; }

    public required string PictureAlt { get; set; }

    public required string PictureTitle { get; set; }

    public required string Keywords { get;  set; }
    
    public required string CreationDate { get;  set; }
    
    public required string MetaDescription { get; set; }
    
    public required string Slug { get; set; }
}