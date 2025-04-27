using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;
public class SaleItemConfiguration : IEntityTypeConfiguration<SaleItem>
{
    public void Configure(EntityTypeBuilder<SaleItem> builder)
    {
        builder.ToTable("SaleItems");

        builder.HasKey(si => si.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.Product)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(si => si.Quantity).IsRequired();
        builder.Property(si => si.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.DiscountAmount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.DiscountPercentage).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.Total).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(si => si.IsCancelled).IsRequired().HasDefaultValue(false);

        builder.HasOne<Sale>()
            .WithMany(s => s.Items)
            .HasForeignKey(x => x.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}