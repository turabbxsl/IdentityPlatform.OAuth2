using IdentityPlatform.Core.Interfaces.Services;
using IdentityPlatform.Core.Models.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace IdentityPlatform.Infrastructure.Security.Jwt
{
    public class JwtTokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public JwtTokenService(IConfiguration config)
        {
            _config = config;
        }

        public (string AccessToken, string IdToken) GenerateTokens(Customer customer, OAuthClient client, IEnumerable<string> scopes)
        {
            var keyString = _config["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessClaims = new List<Claim>
                        {
                            new Claim("sub", customer.Id.ToString()),
                            new Claim("client_id", client.ClientId),
                            new Claim("jti", Guid.NewGuid().ToString()),
                            new Claim("reg_num", customer.RegistrationNumber)
                        };

            foreach (var scope in scopes)
            {
                accessClaims.Add(new Claim("scope", scope));
            }

            int accessExpiration = int.Parse(_config["Jwt:AccessTokenExpirationMinutes"] ?? "60");
            int refreshExpirationDays = int.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "7");

            var accessToken = CreateToken(accessClaims, creds, client.ClientName, TimeSpan.FromMinutes(accessExpiration));

            var idClaims = new List<Claim>()
            {
                new Claim("sub", customer.Id.ToString()),
                new Claim("aud", client.ClientId),
                new Claim("name", customer.FullName),
                new Claim("email", customer.Email ?? ""),
                new Claim("preferred_username", customer.RegistrationNumber),
                new Claim("auth_time", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
            };
            var idToken = CreateToken(idClaims, creds, client.ClientName, TimeSpan.FromDays(refreshExpirationDays));

            return (accessToken, idToken);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _config["Jwt:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"])),

                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParams, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
        !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Keçərsiz token alqoritmi!");
            }

            return principal;
        }


        private string CreateToken(IEnumerable<Claim> claims, SigningCredentials creds, string audience, TimeSpan expiration)
        {
            var tokenDesciption = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(expiration),
                Issuer = _config["Jwt:Issuer"],
                Audience = audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDesciption);

            return tokenHandler.WriteToken(token);
        }
    }
}
