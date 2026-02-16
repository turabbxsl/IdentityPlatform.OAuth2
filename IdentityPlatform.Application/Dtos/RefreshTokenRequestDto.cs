namespace IdentityPlatform.Application.Dtos
{
    public record RefreshTokenRequestDto(string ExpiredAccessToken, string RefreshToken);
}
