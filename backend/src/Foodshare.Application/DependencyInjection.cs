using System.Reflection;
using FluentValidation;
using Foodshare.Application.Common.Behaviours;
using Foodshare.Application.Dishes.Commands.CreateDish;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Foodshare.Application;

public static class DependencyInjection
{
    public static void AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateDishCommand).Assembly);
            cfg.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });
        
        builder.Services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());
    }
}