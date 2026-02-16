using IdentityPlatform.Application.Dtos;
using IdentityPlatform.Core.Common;
using IdentityPlatform.UI.Models;
using System.IdentityModel.Tokens.Jwt;

namespace IdentityPlatform.UI.Services
{
    public interface IAccountService
    {
        Task<Result<ClientDetailsDto>> CheckCustomer(string clientId, string redirectUri);
        Task<Result<string>> LoginAndGetCodeAsync(LoginViewModel model);
        Task<Result<LoginViewModel>> PrepareLoginAsync(string clientId, string redirectUri);
        Task<Result<UserProfileViewModel>> ProcessConsentAsync(ConsentPostModel model);

    }

    public class AccountService(IIdentityPlatformService platformService) : IAccountService
    {

        public async Task<Result<LoginViewModel>> PrepareLoginAsync(string clientId, string redirectUri)
        {
            var response = await platformService.ValidateClientAsync(clientId, redirectUri);

            if (!response.IsSuccess)
                return Result<LoginViewModel>.Failure(response.Errors?.FirstOrDefault());

            return Result<LoginViewModel>.Success(new LoginViewModel
            {
                ClientId = response.Data.ClientId,
                ClientName = response.Data.ClientName,
                RedirectUri = redirectUri
            });
        }

        public async Task<Result<string>> LoginAndGetCodeAsync(LoginViewModel model)
        {
            var loginDto = new LoginRequestDto(model.RegNumber, model.Password, model.ClientId);
            var response = await platformService.AuthorizeAsync(loginDto);

            return response;
        }

        public async Task<Result<ClientDetailsDto>> CheckCustomer(string clientId, string redirectUri)
        {
            var response = await platformService.ValidateClientAsync(clientId, redirectUri);
            return response;
        }

        public async Task<Result<UserProfileViewModel>> ProcessConsentAsync(ConsentPostModel model)
        {
            var tokenRequest = new TokenRequestDto(
                model.Code,
                model.ClientId,
                "secret123");

            var tokenResult = await platformService.GetTokenAsync(tokenRequest);

            if (!tokenResult.IsSuccess)
                return Result<UserProfileViewModel>.Failure(tokenResult.Errors?.FirstOrDefault());

            var profile = MapProfileFromToken(tokenResult.Data);
            return Result<UserProfileViewModel>.Success(profile);
        }


        private UserProfileViewModel MapProfileFromToken(TokenResponseDto tokens)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(tokens.IdToken);

            return new UserProfileViewModel
            {
                Subject = jsonToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value,
                FullName = jsonToken.Claims.FirstOrDefault(c => c.Type == "name")?.Value,
                Email = jsonToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value,
                RegistrationNumber = jsonToken.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value,
                AuthTime = jsonToken.Claims.FirstOrDefault(c => c.Type == "auth_time")?.Value,
                AccessToken = tokens.AccessToken,
                IdToken = tokens.IdToken,
                IsAuthenticated = true
            };
        }

    }
}
