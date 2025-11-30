using AutoMapper;
using Foodshare.Core.Entities;

namespace Foodshare.Application.Common.Users.Queries;

public class UserDto
{
    public Guid Id { get; init; }
    public string FullName { get; init; } = null!;
    public string Email { get; init; } = null!;
    public DateTimeOffset Created { get; init; }
    public int TotalDishes { get; init; }
    public int TotalReservations { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.TotalDishes, opt => opt.MapFrom(s => s.Dishes.Count))
                .ForMember(d => d.TotalReservations, opt => opt.MapFrom(s => s.Reservations.Count));
        }
    }
}
