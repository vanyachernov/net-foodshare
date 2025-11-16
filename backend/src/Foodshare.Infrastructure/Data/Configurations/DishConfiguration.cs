using Foodshare.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodshare.Infrastructure.Data.Configurations;

public class DishConfiguration : IEntityTypeConfiguration<Dish>
{
    public void Configure(EntityTypeBuilder<Dish> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Price)
            .HasPrecision(10, 2);

        builder.Property(x => x.PhotoUrl)
            .IsRequired()
            .HasMaxLength(300);
        
        builder.Property(x => x.Created)
            .IsRequired();
        
        builder.Property(x => x.LastModified)
            .IsRequired();

        builder
            .HasMany(x => x.Reservations)
            .WithOne(x => x.Dish)
            .HasForeignKey(x => x.DishId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}