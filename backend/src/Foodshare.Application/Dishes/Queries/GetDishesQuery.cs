using AutoMapper;
using AutoMapper.QueryableExtensions;
using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Dishes.Queries;

public record GetDishesQuery : IRequest<Result<PaginatedResult<DishDto>>>
{
    public string? SearchTerm { get; init; }
    public DishCategory? Category { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public Guid? OwnerId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetDishesQueryHandler : IRequestHandler<GetDishesQuery, Result<PaginatedResult<DishDto>>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetDishesQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<PaginatedResult<DishDto>>> Handle(GetDishesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Dishes
            .Include(d => d.Owner)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLower();
            query = query.Where(d => 
                d.Title.ToLower().Contains(searchTerm) || 
                d.Description.ToLower().Contains(searchTerm));
        }

        if (request.Category.HasValue)
        {
            query = query.Where(d => d.Category == request.Category.Value);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(d => d.Price >= request.MinPrice.Value);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(d => d.Price <= request.MaxPrice.Value);
        }

        if (request.OwnerId.HasValue)
        {
            query = query.Where(d => d.OwnerId == request.OwnerId.Value);
        }

        // Order by most recent
        query = query.OrderByDescending(d => d.Created);

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var dishes = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<DishDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var paginatedResult = new PaginatedResult<DishDto>(
            dishes,
            totalCount,
            request.PageNumber,
            request.PageSize);

        return Result<PaginatedResult<DishDto>>.Success(paginatedResult);
    }
}
