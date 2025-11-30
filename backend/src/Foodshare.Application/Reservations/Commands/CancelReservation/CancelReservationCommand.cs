using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Reservations.Commands.CancelReservation;

public record CancelReservationCommand : IRequest<Result<bool>>
{
    public Guid ReservationId { get; init; }
    public Guid RequestingUserId { get; init; }
}

public class CancelReservationCommandHandler : IRequestHandler<CancelReservationCommand, Result<bool>>
{
    private readonly IAppDbContext _context;

    public CancelReservationCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(CancelReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _context.Reservations
            .Include(r => r.Dish)
            .FirstOrDefaultAsync(r => r.Id == request.ReservationId, cancellationToken);

        if (reservation == null)
        {
            return Result<bool>.Failure(["Reservation not found."]);
        }

        // User can cancel their own reservation, or dish owner can cancel any reservation
        if (reservation.UserId != request.RequestingUserId && reservation.Dish.OwnerId != request.RequestingUserId)
        {
            return Result<bool>.Failure(["You can only cancel your own reservations."]);
        }

        // Cannot cancel already completed reservations
        if (reservation.Status == ReservationStatus.Completed)
        {
            return Result<bool>.Failure(["Cannot cancel a completed reservation."]);
        }

        reservation.Status = ReservationStatus.Cancelled;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
