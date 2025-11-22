using AutoMapper;
using AutoMapper.QueryableExtensions;
using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Dishes.Queries;

public record GetDishesQuery : IRequest<Result<List<DishDto>>>;

public class GetDishesQueryHandler : IRequestHandler<GetDishesQuery, Result<List<DishDto>>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetDishesQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<List<DishDto>>> Handle(GetDishesQuery request, CancellationToken cancellationToken)
    {
        var dishes = await _context.Dishes
            .ProjectTo<DishDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<DishDto>>.Success(dishes);
    }
}
