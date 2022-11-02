using Microsoft.AspNetCore.Authentication.Cookies;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;

var builder = WebApplication.CreateBuilder(args);

#region AUTH-COOKIE CONFIG
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/access-denied";
                });
#endregion

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<ICatalogService, CatalogService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUser, AspNetUser>();

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseExceptionHandler("/error/500");
app.UseStatusCodePagesWithRedirects("/error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

#region MIDDLEWARES
app.UseMiddleware<ExceptionMiddleware>();
#endregion

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();
