using AutoMapper;
using Foodshare.Core.Entities;
using Foodshare.Core.Enums;

namespace Foodshare.Application.Reservations.Queries;

public class ReservationDto
{
    public Guid Id { get; set; }
    public Guid DishId { get; set; }
    public string DishTitle { get; set; } = null!;
    public decimal DishPrice { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public ReservationStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? PickupTime { get; set; }
    public DateTimeOffset Created { get; set; }
}

public class ReservationDtoMappingProfile : Profile
{
    public ReservationDtoMappingProfile()
    {
        CreateMap<Reservation, ReservationDto>()
            .ForMember(d => d.DishTitle, opt => opt.MapFrom(s => s.Dish.Title))
            .ForMember(d => d.DishPrice, opt => opt.MapFrom(s => s.Dish.Price))
            .ForMember(d => d.UserName, opt => opt.MapFrom(s => s.User.FullName))
            .ForMember(d => d.UserEmail, opt => opt.MapFrom(s => s.User.Email));
    }
}
