using IdentityPlatform.Application.Dtos;
using IdentityPlatform.Core.Common;

namespace IdentityPlatform.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<string>> GenerateAuthorizationCodeAsync(LoginRequestDto loginRequest);
        Task<Result<TokenResponseDto>> ExchangeCodeForTokensAsync(TokenRequestDto tokenRequest);
        Task<Result<TokenResponseDto>> RefreshAsync(RefreshTokenRequestDto request);

        Task<Result<ClientDetailsDto>> ValidateClientAsync(string clientId, string redirectUri);
    }
}
