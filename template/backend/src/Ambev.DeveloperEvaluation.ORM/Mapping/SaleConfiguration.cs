using Ambev.DeveloperEvaluation.Domain.Sales.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;
public class SaleConfiguration : IEntityTypeConfiguration<Sale>
{
    public void Configure(EntityTypeBuilder<Sale> builder)
    {
        builder.ToTable("Sales");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(s => s.SaleNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(s => s.SaleNumber).IsUnique();

        builder.Property(s => s.Date).IsRequired();
        builder.Property(s => s.IsCancelled).IsRequired();

        builder.Property(u => u.Customer)
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(u => u.Branch)
            .HasConversion<string>()
            .HasMaxLength(30);


        builder.HasMany(s => s.Items)
            .WithOne()
            .HasForeignKey(x => x.SaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}