using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Entities;
using MediatR;

namespace Foodshare.Application.Reservations.Commands.CreateReservation;

public record CreateReservationCommand : IRequest<Result<Guid?>>
{
    public Guid DishId { get; set; }
    public Guid UserId { get; set; }
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
        
        var entity = new Reservation
        {
            DishId = request.DishId,
            UserId = request.UserId
        };

        _context.Reservations.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid?>.Success(entity.Id);
    }
}
