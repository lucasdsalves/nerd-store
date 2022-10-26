using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NSE.Catalog.API.Data;
using NSE.Catalog.API.Data.Repository;
using NSE.Catalog.API.Models;
using NSE.WebAPI.Core.Identities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

#region JWT
builder.Services.AddJwtConfiguration(builder);
#endregion

builder.Services.AddCors(options => {
    options.AddPolicy("FullAccess",
        c => c.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
});

builder.Services.AddControllers();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<CatalogContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "NerdStore Enterprise Catalog API",
        Description = "This API is part of ASP.NET Core Enterprise Applications course at desenvolvedor.io",
        Contact = new OpenApiContact() { Name = "@lucasdsalves", Email = "contact.lucasdsalves@gmail.com" },
        License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
    });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Insert JWT token as the following - Bearer {your token}",
        Name = "Authorization",
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("FullAccess");

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
