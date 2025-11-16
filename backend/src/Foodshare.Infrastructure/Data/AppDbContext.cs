using Foodshare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Foodshare.Infrastructure.Data;

public class AppDbContext : DbContext, IAppDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    // public DbSet<User> Users => Set<User>();
    // public DbSet<Profile> Profiles => Set<Profile>();
    // public DbSet<Listing> Listings => Set<Listing>();
    // public DbSet<Booking> Bookings => Set<Booking>();
    // public DbSet<Media> Media => Set<Media>();
    // public DbSet<Category> Categories => Set<Category>();
    // public DbSet<Tag> Tags => Set<Tag>();
    // public DbSet<Review> Reviews => Set<Review>();
    // public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    // public DbSet<Report> Reports => Set<Report>();
    // public DbSet<Notification> Notifications => Set<Notification>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}