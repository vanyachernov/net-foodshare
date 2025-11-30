using FluentValidation;
using Foodshare.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Reservations.Commands.CreateReservation;

public class CreateReservationValidator : AbstractValidator<CreateReservationCommand>
{
    private readonly IAppDbContext _context;

    public CreateReservationValidator(IAppDbContext context)
    {
        _context = context;
        
        RuleFor(x => x.DishId)
            .NotEmpty().WithMessage("DishId is required.")
            .MustAsync(DishExists)
            .WithMessage("Dish with Id '{PropertyValue}' does not exist.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.")
            .MustAsync(UserExists)
            .WithMessage("User with Id '{PropertyValue}' does not exist.");

        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("Notes cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Notes));

        RuleFor(x => x.PickupTime)
            .GreaterThan(DateTime.UtcNow).WithMessage("Pickup time must be in the future.")
            .When(x => x.PickupTime.HasValue);
    }
    
    private async Task<bool> DishExists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Dishes
            .AnyAsync(l => l.Id == id, cancellationToken);
    }

    private async Task<bool> UserExists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(l => l.Id == id, cancellationToken);
    }
}
