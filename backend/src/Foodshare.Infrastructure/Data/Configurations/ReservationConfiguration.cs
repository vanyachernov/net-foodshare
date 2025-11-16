using Foodshare.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Foodshare.Infrastructure.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(x => x.Id);

        builder
            .HasOne(x => x.Dish)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.DishId);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.Reservations)
            .HasForeignKey(x => x.UserId);
        
        builder.Property(x => x.Created)
            .IsRequired();
        
        builder.Property(x => x.LastModified)
            .IsRequired();
    }
}