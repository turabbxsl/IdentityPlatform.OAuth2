using IdentityPlatform.Application.Dtos;
using IdentityPlatform.Application.Interfaces;
using IdentityPlatform.Application.Mapping;
using IdentityPlatform.Core.Common;
using IdentityPlatform.Core.Interfaces;
using IdentityPlatform.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace IdentityPlatform.Application.Services
{
    public class AuthService(
                            IUnitOfWork unitOfWork,
                            ITokenService tokenService,
                            IConfiguration configuration
                                        ) : IAuthService
    {
        public async Task<Result<string>> GenerateAuthorizationCodeAsync(LoginRequestDto loginRequest)
        {
            try
            {
                var customer = await unitOfWork.Customers.GetByRegistrationNumberAsync(loginRequest.RegistrationNumber);

                if (customer == null || customer.PasswordHash != loginRequest.Password)
                    return Result<string>.Failure("The login information is incorrect.", 401);

                var authCode = GenerateAuthCode();
                var codeEntity = AutoMapping.ToAuthCodeEntity(loginRequest, customer.Id, authCode);

                await unitOfWork.AuthCodes.AddAsync(codeEntity);
                await unitOfWork.SaveChangesAsync();

                return Result<string>.Success(authCode, 200);
            }
            catch (Exception ex)
            {
                return Result<string>.Failure($"Error while generating code: {ex.Message}", 500);
            }
        }

        public async Task<Result<TokenResponseDto>> ExchangeCodeForTokensAsync(TokenRequestDto tokenRequest)
        {
            try
            {
                var savedCode = await unitOfWork.AuthCodes.GetAsync(c =>
                    c.Code == tokenRequest.code && !c.IsUsed && c.ExpiresAt > DateTime.UtcNow);

                if (savedCode == null)
                    return Result<TokenResponseDto>.Failure("The code is invalid or has expired.", 400);

                var client = await unitOfWork.Clients.GetByClientIdAsync(tokenRequest.clientId);
                if (client == null || client.ClientSecret != tokenRequest.clientSecret)
                    return Result<TokenResponseDto>.Failure("Client not verified!", 401);

                savedCode.IsUsed = true;
                unitOfWork.AuthCodes.Update(savedCode);

                var customer = await unitOfWork.Customers.GetByIdAsync(savedCode.CustomerId);
                var tokens = tokenService.GenerateTokens(customer, client, client.AllowedScopes.Split(','));

                var refreshToken = tokenService.GenerateRefreshToken();
                var refreshTokenEntity = AutoMapping.ToRefreshTokenEntity(refreshToken, customer.Id, client.ClientId);

                await unitOfWork.RefreshTokens.AddAsync(refreshTokenEntity);
                await unitOfWork.SaveChangesAsync();

                var accessExpirationMinutes = int.Parse(configuration["Jwt:AccessTokenExpirationMinutes"] ?? "60");
                var expiresInSeconds = accessExpirationMinutes * 60;

                var response = new TokenResponseDto
                {
                    AccessToken = tokens.AccessToken,
                    IdToken = tokens.IdToken,
                    RefreshToken = refreshToken,
                    ExpiresInSeconds = expiresInSeconds
                };

                return Result<TokenResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<TokenResponseDto>.Failure(ex.Message, 500);
            }
        }

        public async Task<Result<TokenResponseDto>> RefreshAsync(RefreshTokenRequestDto request)
        {
            try
            {
                var principal = tokenService.GetPrincipalFromExpiredToken(request.ExpiredAccessToken);
                var customerIdClaim = principal.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                if (string.IsNullOrEmpty(customerIdClaim))
                    return Result<TokenResponseDto>.Failure("Customer information was not found within the token.", 401);

                var customerId = Guid.Parse(customerIdClaim);
                var savedRefreshToken = await unitOfWork.RefreshTokens.GetAsync(t =>
                                            t.Token == request.RefreshToken &&
                                            t.CustomerId == customerId &&
                                            !t.IsRevoked);

                if (savedRefreshToken == null)
                    return Result<TokenResponseDto>.Failure("The session was not found or was canceled.", 401);

                if (DateTime.UtcNow >= savedRefreshToken.ExpiresAt)
                    return Result<TokenResponseDto>.Failure("Your session has expired, please log in again.", 401);

                var customer = await unitOfWork.Customers.GetByIdAsync(customerId);
                var client = await unitOfWork.Clients.GetByClientIdAsync(savedRefreshToken.ClientId);

                var tokens = tokenService.GenerateTokens(customer, client, client.AllowedScopes.Split(','));

                var response = new TokenResponseDto
                {
                    AccessToken = tokens.AccessToken,
                    IdToken = tokens.IdToken,
                    RefreshToken = savedRefreshToken.Token
                };

                return Result<TokenResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                return Result<TokenResponseDto>.Failure(ex.Message, 500);
            }
        }

        public async Task<Result<ClientDetailsDto>> ValidateClientAsync(string clientId, string redirectUri)
        {
            var client = await unitOfWork.Clients.GetByClientIdAsync(clientId);

            if (client == null)
                return Result<ClientDetailsDto>.Failure("Client not found.", 404);

            if (client.RedirectUri != redirectUri)
                return Result<ClientDetailsDto>.Failure("Redirect URI is not valid.", 400);

            var clientDetails = new ClientDetailsDto
            {
                ClientId = client.ClientId,
                ClientName = client.ClientName
            };

            return Result<ClientDetailsDto>.Success(clientDetails);
        }


        private string GenerateAuthCode(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
