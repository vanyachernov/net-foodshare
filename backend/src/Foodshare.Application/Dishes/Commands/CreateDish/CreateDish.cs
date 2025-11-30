using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Entities;
using Foodshare.Core.Enums;
using MediatR;

namespace Foodshare.Application.Dishes.Commands.CreateDish;

public record CreateDishCommand : IRequest<Result<Guid?>>
{
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public DishCategory Category { get; init; }
    public string PhotoUrl { get; init; } = null!;
    public Guid OwnerId { get; init; }
    public bool IsAvailable { get; init; } = true;
    public int? QuantityAvailable { get; init; }
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
        var user = await _context.Users.FindAsync([request.OwnerId], cancellationToken);

        if (user == null)
        {
            return Result<Guid?>.Failure(["User not found."]);
        }
        
        var entity = new Dish
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            PhotoUrl = request.PhotoUrl,
            OwnerId = request.OwnerId,
            IsAvailable = request.IsAvailable,
            QuantityAvailable = request.QuantityAvailable
        };

        _context.Dishes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid?>.Success(entity.Id);
    }
}
