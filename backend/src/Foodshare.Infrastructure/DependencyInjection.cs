using Foodshare.Application.Common.Interfaces;
using Foodshare.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Foodshare.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("AppDb");
        
        builder.Services.AddDbContext<AppDbContext>((options) =>
        {
            options.UseNpgsql(connectionString);
        });

        builder.Services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
    }
}