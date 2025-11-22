namespace Foodshare.Application.Common.Interfaces;

public interface IAuthService
{
    string GenerateToken(Guid userId, string email, string fullName);
}