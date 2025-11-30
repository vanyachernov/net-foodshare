using AutoMapper;
using AutoMapper.QueryableExtensions;
using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Reservations.Queries.GetDishReservations;

public record GetDishReservationsQuery(Guid DishId) : IRequest<Result<List<ReservationDto>>>;

public class GetDishReservationsQueryHandler : IRequestHandler<GetDishReservationsQuery, Result<List<ReservationDto>>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetDishReservationsQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<ReservationDto>>> Handle(GetDishReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _context.Reservations
            .Include(r => r.Dish)
            .Include(r => r.User)
            .Where(r => r.DishId == request.DishId)
            .OrderByDescending(r => r.Created)
            .ProjectTo<ReservationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<ReservationDto>>.Success(reservations);
    }
}
