using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Dishes.Commands.UpdateDish;

public record UpdateDishCommand : IRequest<Result<bool>>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public DishCategory Category { get; init; }
    public string PhotoUrl { get; init; } = null!;
    public bool IsAvailable { get; init; }
    public int? QuantityAvailable { get; init; }
    public Guid RequestingUserId { get; init; }
}

public class UpdateDishCommandHandler : IRequestHandler<UpdateDishCommand, Result<bool>>
{
    private readonly IAppDbContext _context;

    public UpdateDishCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(UpdateDishCommand request, CancellationToken cancellationToken)
    {
        var dish = await _context.Dishes
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (dish == null)
        {
            return Result<bool>.Failure(["Dish not found."]);
        }

        // Only owner can update their dish
        if (dish.OwnerId != request.RequestingUserId)
        {
            return Result<bool>.Failure(["You can only update your own dishes."]);
        }

        dish.Title = request.Title;
        dish.Description = request.Description;
        dish.Price = request.Price;
        dish.Category = request.Category;
        dish.PhotoUrl = request.PhotoUrl;
        dish.IsAvailable = request.IsAvailable;
        dish.QuantityAvailable = request.QuantityAvailable;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
