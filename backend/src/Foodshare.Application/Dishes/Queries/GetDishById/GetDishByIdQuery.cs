using AutoMapper;
using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Dishes.Queries.GetDishById;

public record GetDishByIdQuery(Guid Id) : IRequest<Result<DishDto?>>;

public class GetDishByIdQueryHandler : IRequestHandler<GetDishByIdQuery, Result<DishDto?>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetDishByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<DishDto?>> Handle(GetDishByIdQuery request, CancellationToken cancellationToken)
    {
        var dish = await _context.Dishes
            .Include(d => d.Owner)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (dish == null)
        {
            return Result<DishDto?>.Failure(["Dish not found."]);
        }

        var dto = _mapper.Map<DishDto>(dish);
        
        return Result<DishDto?>.Success(dto);
    }
}
