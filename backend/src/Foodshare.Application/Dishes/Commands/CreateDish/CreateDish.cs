using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Entities;
using MediatR;

namespace Foodshare.Application.Dishes.Commands.CreateDish;

public record CreateDishCommand : IRequest<Result<Guid?>>
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public string Category { get; init; } = null!;
    public string PhotoUrl { get; init; } = null!;
    public Guid OwnerId { get; init; }
}

public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, Result<Guid?>>
{
    private readonly IAppDbContext _context;

    public CreateDishCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid?>> Handle(CreateDishCommand request, CancellationToken cancellationToken)
    {
        var entity = new Dish
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            PhotoUrl = request.PhotoUrl,
            OwnerId = request.OwnerId
        };

        _context.Dishes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid?>.Success(entity.Id);
    }
}
