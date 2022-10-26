using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSE.WebAPI.Core.Extensions;
using System.Text;

namespace NSE.WebAPI.Core.Identities
{
    public static class AuthConfig
    {
        public static void AddJwtConfiguration(this IServiceCollection serviceCollection, WebApplicationBuilder builder)
        {
            var appSettingsSection = builder.Configuration.GetSection("AppSettings");
            builder.Services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = true;
                bearerOptions.SaveToken = true;
                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = appSettings.ValidIn,
                    ValidIssuer = appSettings.Issuer
                };
            });
        }
    }
}
