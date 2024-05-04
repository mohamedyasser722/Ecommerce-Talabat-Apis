using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configurations
{
    internal class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(P => P.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(P => P.Description)
                .IsRequired();

            builder.Property(P => P.PictureUrl)
                .IsRequired();

            builder.Property(P => P.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(P => P.ProductCategory)
                    .WithMany()
                    .HasForeignKey(P => P.CategoryId);

            builder.HasOne(P => P.Brand)
                   .WithMany()
                   .HasForeignKey(P => P.BrandId);
        }
    }
}
