using FluentValidation;
using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    private readonly IAppDbContext _context;
    
    public CreateDishCommandValidator(IAppDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
            .WithMessage("'{PropertyName}' must be unique.")
            .WithErrorCode("Unique");

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(60)
                .WithMessage("'{PropertyName}' must be at least {MinLength} characters long.")
            .MaximumLength(1000);

        RuleFor(x => x.Category)
            .NotEmpty();

        RuleFor(x => x.Price)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.PhotoUrl)
            .NotEmpty();

        RuleFor(x => x.OwnerId)
            .NotEmpty()
            .MustAsync(UserExists)
                .WithMessage("User with Id '{PropertyValue}' does not exist.");
    }

    private async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return !await _context.Dishes
            .AnyAsync(l => l.Title == title, cancellationToken);
    }   
    
    private async Task<bool> UserExists(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users
            .AnyAsync(l => l.Id == id, cancellationToken);
    }   
}