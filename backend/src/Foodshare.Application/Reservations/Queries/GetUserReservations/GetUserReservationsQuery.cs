using AutoMapper;
using AutoMapper.QueryableExtensions;
using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Reservations.Queries.GetUserReservations;

public record GetUserReservationsQuery(Guid UserId) : IRequest<Result<List<ReservationDto>>>;

public class GetUserReservationsQueryHandler : IRequestHandler<GetUserReservationsQuery, Result<List<ReservationDto>>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserReservationsQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<ReservationDto>>> Handle(GetUserReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _context.Reservations
            .Include(r => r.Dish)
            .Include(r => r.User)
            .Where(r => r.UserId == request.UserId)
            .OrderByDescending(r => r.Created)
            .ProjectTo<ReservationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<ReservationDto>>.Success(reservations);
    }
}
