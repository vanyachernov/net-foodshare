using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Common.Users.Commands.UpdateProfile;

public record UpdateUserProfileCommand : IRequest<Result<bool>>
{
    public Guid UserId { get; init; }
    public string FullName { get; init; } = null!;
    public string Email { get; init; } = null!;
}

public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Result<bool>>
{
    private readonly IAppDbContext _context;

    public UpdateUserProfileCommandHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            return Result<bool>.Failure(["User not found."]);
        }

        // Check if email is already taken by another user
        var emailExists = await _context.Users
            .AnyAsync(u => u.Email == request.Email && u.Id != request.UserId, cancellationToken);

        if (emailExists)
        {
            return Result<bool>.Failure(["Email is already taken."]);
        }

        user.FullName = request.FullName;
        user.Email = request.Email;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}
