using IdentityPlatform.Core.Interfaces;
using IdentityPlatform.Core.Interfaces.Services;
using IdentityPlatform.Infrastructure.Persistence.Context;
using IdentityPlatform.Infrastructure.Persistence.Repositories;
using IdentityPlatform.Infrastructure.Persistence.UnitOfWork;
using IdentityPlatform.Infrastructure.Security.Jwt;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityPlatform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(
       this IServiceCollection services,
       IConfiguration config)
        {
            services.AddDbContext<PlatformContext>(options =>
                options.UseNpgsql(
                    config.GetConnectionString("Default")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IOAuthClientRepository, OAuthClientRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ITokenService, JwtTokenService>();

            return services;
        }
    }
}
