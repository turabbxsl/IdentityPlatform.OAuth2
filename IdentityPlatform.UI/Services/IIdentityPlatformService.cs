using IdentityPlatform.Application.Dtos;
using IdentityPlatform.Core.Common;

namespace IdentityPlatform.UI.Services
{
    public interface IIdentityPlatformService
    {
        Task<Result<ClientDetailsDto>> ValidateClientAsync(string clientId, string redirectUri);

        Task<Result<string>> AuthorizeAsync(LoginRequestDto loginRequest);

        Task<Result<TokenResponseDto>> GetTokenAsync(TokenRequestDto tokenRequest);
    }

    public class IdentityPlatformService : IIdentityPlatformService
    {
        private readonly HttpClient _httpClient;

        public IdentityPlatformService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<ClientDetailsDto>> ValidateClientAsync(string clientId, string redirectUri)
        {
            var response = await _httpClient.GetAsync($"api/auth/validate-client?clientId={clientId}&redirectUri={redirectUri}");
            return await response.Content.ReadFromJsonAsync<Result<ClientDetailsDto>>();
        }

        public async Task<Result<string>> AuthorizeAsync(LoginRequestDto loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/authorize", loginRequest);
            return await response.Content.ReadFromJsonAsync<Result<string>>();
        }

        public async Task<Result<TokenResponseDto>> GetTokenAsync(TokenRequestDto tokenRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/token", tokenRequest);
            return await  response.Content.ReadFromJsonAsync<Result<TokenResponseDto>>(); ;
        }
    }
}
