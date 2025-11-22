using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Common.Users.Login;

public record LoginUserCommand(string Email, string Password) : IRequest<Result<string>>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<string>>
{
    private readonly IAppDbContext _context;
    private readonly IAuthService _jwt;

    public LoginUserCommandHandler(IAppDbContext context, IAuthService jwt)
    {
        _context = context;
        _jwt = jwt;
    }

    public async Task<Result<string>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user == null)
        {
            return Result<string>.Failure(new[] { "Invalid email or password." });
        }

        var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!passwordValid)
        {
            return Result<string>.Failure(new[] { "Invalid email or password." });
        }

        var token = _jwt.GenerateToken(user.Id, user.Email, user.FullName);

        return Result<string>.Success(token);
    }
}