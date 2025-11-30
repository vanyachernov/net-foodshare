using AutoMapper;
using Foodshare.Application.Common.Interfaces;
using Foodshare.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Foodshare.Application.Common.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<Result<UserDto?>>;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto?>>
{
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IAppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<UserDto?>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Dishes)
            .Include(u => u.Reservations)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
        {
            return Result<UserDto?>.Failure(["User not found."]);
        }

        var dto = _mapper.Map<UserDto>(user);

        return Result<UserDto?>.Success(dto);
    }
}
