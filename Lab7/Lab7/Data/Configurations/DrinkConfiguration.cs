using Lab7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lab7.Data.Configurations;

public class DrinkConfiguration : IEntityTypeConfiguration<Drink>
{
    public void Configure(EntityTypeBuilder<Drink> builder)
    {
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(100);

        builder.HasOne(d => d.Manufacturer)
               .WithMany(m => m.Drinks)
               .HasForeignKey(d => d.ManufacturerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
