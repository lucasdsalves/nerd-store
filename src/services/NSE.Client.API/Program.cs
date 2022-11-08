using Microsoft.EntityFrameworkCore;
using NSE.Client.API.Data;

var builder = WebApplication.CreateBuilder(args);

#region DB-CONTEXT
builder.Services.AddDbContext<ClientsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
