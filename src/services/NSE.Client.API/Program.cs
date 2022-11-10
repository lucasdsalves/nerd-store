using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

builder.Services.AddScoped<IClientsRepository, ClientsRepository>();
builder.Services.AddScoped<ClientsContext>();
builder.Services.AddScoped<IMediatorHandler, MediatorHandler>();
builder.Services.AddScoped<IRequestHandler<RegisterClientCommand, ValidationResult>, ClientCommandHandler>();
builder.Services.AddScoped<INotificationHandler<RegisteredClientEvent>, ClientEventHandler>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
