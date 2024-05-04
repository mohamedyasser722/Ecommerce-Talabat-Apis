using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            
            builder.OwnsOne(O => O.ShippingAddress, SA => SA.WithOwner()); // 1 To 1 Total

            builder.Property(O => O.Status)
                .HasConversion(
                OS => OS.ToString(),
                OS => (OrderStatus)Enum.Parse(typeof(OrderStatus), OS));

            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(O => O.Items)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
