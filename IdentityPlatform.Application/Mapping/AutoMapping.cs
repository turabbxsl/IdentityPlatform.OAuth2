using IdentityPlatform.Application.Dtos;
using IdentityPlatform.Core.Models.Entities;

namespace IdentityPlatform.Application.Mapping
{
    public static class AutoMapping
    {
        public static AuthCode ToAuthCodeEntity(this LoginRequestDto dto, Guid customerId, string authCode)
        {
            return new AuthCode
            {
                Code = authCode,
                CustomerId = customerId,
                ClientId = dto.ClientId,
                ExpiresAt = DateTime.UtcNow.AddMinutes(5),
                IsUsed = false
            };
        }

        public static RefreshToken ToRefreshTokenEntity(string token, Guid customerId, string clientId)
        {
            return new RefreshToken
            {
                Token = token,
                CustomerId = customerId,
                ClientId = clientId,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };
        }
    }
}
