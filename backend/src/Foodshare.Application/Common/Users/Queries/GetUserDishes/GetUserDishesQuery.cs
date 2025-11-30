using AutoMapper;
using AutoMapper.QueryableExtensions;
using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Application.Dishes.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Common.Users.Queries.GetUserDishes;

public record GetUserDishesQuery(Guid UserId) : IRequest<Result<List<DishDto>>>;

public class GetUserDishesQueryHandler : IRequestHandler<GetUserDishesQuery, Result<List<DishDto>>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserDishesQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<DishDto>>> Handle(GetUserDishesQuery request, CancellationToken cancellationToken)
    {
        var dishes = await _context.Dishes
            .Include(d => d.Owner)
            .Where(d => d.OwnerId == request.UserId)
            .OrderByDescending(d => d.Created)
            .ProjectTo<DishDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<DishDto>>.Success(dishes);
    }
}
