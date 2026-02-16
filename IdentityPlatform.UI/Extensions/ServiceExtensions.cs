using IdentityPlatform.UI.Services;

namespace IdentityPlatform.UI.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            var apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5170/";

            services.AddHttpClient<IIdentityPlatformService, IdentityPlatformService>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
