using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Reservations.Commands.UpdateReservationStatus;

public record UpdateReservationStatusCommand : IRequest<Result<bool>>
{
    public Guid ReservationId { get; init; }
    public ReservationStatus Status { get; init; }
    public Guid RequestingUserId { get; init; }
}

public class UpdateReservationStatusCommandHandler : IRequestHandler<UpdateReservationStatusCommand, Result<bool>>
{
    private readonly IAppDbContext _context;

    public UpdateReservationStatusCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(UpdateReservationStatusCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Dish)
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

        if (reservation == null)
        {
            return Result<bool>.Failure(["Reservation not found."]);
        }

        // Only dish owner can confirm reservations
        if (request.Status == ReservationStatus.Confirmed && reservation.Dish.OwnerId != request.RequestingUserId)
        {
            return Result<bool>.Failure(["Only the dish owner can confirm reservations."]);
        }

        // User can cancel their own reservation, or dish owner can cancel any reservation
        if (request.Status == ReservationStatus.Cancelled)
        {
            if (reservation.UserId != request.RequestingUserId && reservation.Dish.OwnerId != request.RequestingUserId)
            {
                return Result<bool>.Failure(["You can only cancel your own reservations."]);
            }
        }

        reservation.Status = request.Status;

        if (request.Status == ReservationStatus.Confirmed)
        {
            reservation.ConfirmedAt = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
