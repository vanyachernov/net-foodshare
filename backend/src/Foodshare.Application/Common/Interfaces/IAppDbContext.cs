using Foodshare.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Dish> Dishes { get; }
    DbSet<Reservation> Reservations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}