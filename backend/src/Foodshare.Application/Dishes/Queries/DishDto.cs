using AutoMapper;
using Foodshare.Core.Entities;

namespace Foodshare.Application.Dishes.Queries;

public class DishDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public decimal Price { get; init; }
    public string Category { get; init; } = null!;
    public string PhotoUrl { get; init; } = null!;
    public Guid OwnerId { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Dish, DishDto>();
        }
    }
}
