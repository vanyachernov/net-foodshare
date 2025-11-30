using AutoMapper;
using Foodshare.Core.Entities;
using Foodshare.Core.Enums;

namespace Foodshare.Application.Dishes.Queries;

public class DishDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public DishCategory Category { get; init; }
    public string PhotoUrl { get; init; } = null!;
    public Guid OwnerId { get; init; }
    public string OwnerName { get; init; } = null!;
    public DateTimeOffset Created { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Dish, DishDto>()
                .ForMember(d => d.OwnerName, opt => opt.MapFrom(s => s.Owner.FullName));
        }
    }
}
