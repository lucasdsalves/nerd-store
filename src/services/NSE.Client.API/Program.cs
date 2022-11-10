using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NSE.Client.API.Application.Commands;
using NSE.Client.API.Data;
using NSE.Client.API.Data.Repository;
using NSE.Client.API.Events;
using NSE.Client.API.Models;
using NSE.Core.Mediator;

var builder = WebApplication.CreateBuilder(args);

#region DB-CONTEXT
builder.Services.AddDbContext<ClientsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region DI
builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<ClientsContext>();
builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<RegisterClientCommand, ValidationResult>, ClientCommandHandler>();
builder.Services.AddScoped<INotificationHandler<RegisteredClientEvent>, ClientEventHandler>();
#endregion

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NerdStore Enterprise Clients API",
        Description = "This API is part of ASP.NET Core Enterprise Applications course at desenvolvedor.io",
        Contact = new OpenApiContact() { Name = "@lucasdsalves", Email = "contact.lucasdsalves@gmail.com" },
        License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
    });
});

#region MEDIATR
builder.Services.AddMediatR(typeof(Program));
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
