using IdentityPlatform.Application.Interfaces;
using IdentityPlatform.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityPlatform.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(
       this IServiceCollection services,
       IConfiguration config)
        {

            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

    }
}
