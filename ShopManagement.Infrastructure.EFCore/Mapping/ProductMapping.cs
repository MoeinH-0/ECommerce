using System.Data.Entity.ModelConfiguration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Infrastructure.EFCore.Mapping;

public class ProductMapping : EntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Code).IsRequired();
        builder.Property(x => x.ShortDescription).IsRequired();
        builder.Property(x => x.Keywords).IsRequired();
        builder.Property(x => x.MetaDescription).IsRequired();
        builder.Property(x => x.Slug).IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId);
        
        builder.HasMany(x => x.ProductPictures)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId);
    }
}