using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Dishes.Commands.DeleteDish;

public record DeleteDishCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
    public Guid RequestingUserId { get; init; }
}

public class DeleteDishCommandHandler : IRequestHandler<DeleteDishCommand, Result<bool>>
{
    private readonly IAppDbContext _context;

    public DeleteDishCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteDishCommand request, CancellationToken cancellationToken)
    {
        var dish = await _context.Dishes
            .Include(d => d.Reservations)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (dish == null)
        {
            return Result<bool>.Failure(["Dish not found."]);
        }

        // Only owner can delete their dish
        if (dish.OwnerId != request.RequestingUserId)
        {
            return Result<bool>.Failure(["You can only delete your own dishes."]);
        }

        // Check for active reservations
        var hasActiveReservations = dish.Reservations.Any(r => 
            r.Status == ReservationStatus.Pending || 
            r.Status == ReservationStatus.Confirmed);

        if (hasActiveReservations)
        {
            return Result<bool>.Failure(["Cannot delete dish with active reservations. Please cancel or complete all reservations first."]);
        }

        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
