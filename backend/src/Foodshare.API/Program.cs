using Foodshare.API.Endpoints;
using Foodshare.Application;
using Foodshare.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    
    builder.AddApplicationServices();
    builder.AddInfrastructureServices();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app
        .MapDishEndpoints()
        .MapUserEndpoints();
    app.Run();
}