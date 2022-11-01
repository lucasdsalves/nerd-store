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
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IUser, AspNetUser>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
