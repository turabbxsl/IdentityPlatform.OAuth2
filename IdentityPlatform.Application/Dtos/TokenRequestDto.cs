namespace IdentityPlatform.Application.Dtos
{
    public record TokenRequestDto(string code,string clientId,string clientSecret);
}
