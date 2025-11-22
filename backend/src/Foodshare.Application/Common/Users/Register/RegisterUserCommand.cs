using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using Foodshare.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Common.Users.Register;

public record RegisterUserCommand(string FullName, string Email, string Password) : IRequest<Result<Guid>>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    private readonly IAppDbContext _context;

    public RegisterUserCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await _context.Users.AnyAsync(x => x.Email == request.Email, cancellationToken);
        
        if (emailExists)
        {
            return Result<Guid>.Failure(new List<string> { "Email is already registered." });
        }

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(user.Id);
    }
}
