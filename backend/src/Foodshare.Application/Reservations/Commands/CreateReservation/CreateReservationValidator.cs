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
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .MustAsync(UserExists)
            .WithMessage("User with Id '{PropertyValue}' does not exist.");
        
        RuleFor(x => x.DishId)
            .NotEmpty()
            .MustAsync(DishExists)
            .WithMessage("Dish with Id '{PropertyValue}' does not exist.");
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
