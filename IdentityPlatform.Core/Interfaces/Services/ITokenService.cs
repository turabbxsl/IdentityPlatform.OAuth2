using IdentityPlatform.Core.Models.Entities;
using System.Security.Claims;

namespace IdentityPlatform.Core.Interfaces.Services
{
    public interface ITokenService
    {
        (string AccessToken, string IdToken) GenerateTokens(Customer customer, OAuthClient client, IEnumerable<string> scopes);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
