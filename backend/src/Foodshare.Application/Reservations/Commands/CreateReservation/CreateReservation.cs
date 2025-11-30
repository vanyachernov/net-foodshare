using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Reservations.Commands.CreateReservation;

public record CreateReservationCommand : IRequest<Result<Guid?>>
{
    public Guid DishId { get; set; }
    public Guid UserId { get; set; }
    public string? Notes { get; set; }
    public DateTime? PickupTime { get; set; }
}

public class CreateReservationCommandHandler : IRequestHandler<CreateReservationCommand, Result<Guid?>>
{
    private readonly IAppDbContext _context;

    public CreateReservationCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid?>> Handle(CreateReservationCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync([request.UserId], cancellationToken);

        if (user == null)
        {
            return Result<Guid?>.Failure(["User not found."]);
        }

        var dish = await _context.Dishes.FindAsync([request.DishId], cancellationToken);

        if (dish == null)
        {
            return Result<Guid?>.Failure(["Dish not found."]);
        }

        // Check if dish is available
        if (!dish.IsAvailable)
        {
            return Result<Guid?>.Failure(["This dish is no longer available."]);
        }

        // Check if quantity is available
        if (dish.QuantityAvailable.HasValue && dish.QuantityAvailable.Value <= 0)
        {
            return Result<Guid?>.Failure(["This dish is out of stock."]);
        }

        // Check if user already has a pending or confirmed reservation for this dish
        var existingReservation = await _context.Reservations
            .AnyAsync(r => r.DishId == request.DishId && 
                          r.UserId == request.UserId && 
                          (r.Status == Core.Enums.ReservationStatus.Pending || 
                           r.Status == Core.Enums.ReservationStatus.Confirmed), 
                      cancellationToken);

        if (existingReservation)
        {
            return Result<Guid?>.Failure(["You already have an active reservation for this dish."]);
        }
        
        var entity = new Reservation
        {
            DishId = request.DishId,
            UserId = request.UserId,
            Notes = request.Notes,
            PickupTime = request.PickupTime
        };

        _context.Reservations.Add(entity);

        // Update dish quantity if applicable
        if (dish.QuantityAvailable.HasValue)
        {
            dish.QuantityAvailable -= 1;
            
            // Mark as unavailable if quantity reaches 0
            if (dish.QuantityAvailable <= 0)
            {
                dish.IsAvailable = false;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid?>.Success(entity.Id);
    }
}
