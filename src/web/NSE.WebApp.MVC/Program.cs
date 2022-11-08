using Microsoft.AspNetCore.Authentication.Cookies;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using Polly.Extensions.Http;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

#region AUTH-COOKIE CONFIG
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/access-denied";
                });
#endregion

#region RETRY-POLICY
var retryWaitPolicy = HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .WaitAndRetryAsync(new[]
                        {
                            TimeSpan.FromSeconds(1),
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10),
                        }, (outcome, timespan, retryCount, context) =>
                        {
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.WriteLine($"Trying for the {retryCount} time.");
                            Console.ForegroundColor= ConsoleColor.DarkGray;
                        });
#endregion

#region SERVICES
builder.Services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

builder.Services.AddHttpClient<IAuthService, AuthService>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>() // TOKEN HTTP
                /*.AddTransientHttpErrorPolicy(p => p.WaitAndRetry(3, _ => TimeSpan.FromMilliseconds(600));*/ // POLLY EXAMPLE 1
                .AddPolicyHandler(retryWaitPolicy) // POLLY EXAMPLE 2
                .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30))); // CIRCUIT BREAKER

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddScoped<IUser, AspNetUser>();

#region REFIT
//builder.Services.AddHttpClient("Refit", options =>
//{
//    options.BaseAddress = new Uri(builder.Configuration.GetSection("AppSettings").Get<AppSettings>().CatalogApiUrl);
//})
//                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
//                .AddTypedClient(Refit.RestService.For<ICatalogServiceRefit>);
#endregion

#endregion

#region APPSETTINGS
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
#endregion

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

#region CULTURE
var supporterCultures = new[] { new CultureInfo("en-US") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US"),
    SupportedCultures = supporterCultures,
    SupportedUICultures = supporterCultures
});
#endregion

#region MIDDLEWARES
app.UseMiddleware<ExceptionMiddleware>();
#endregion

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();
