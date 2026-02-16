namespace IdentityPlatform.Application.Dtos
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public string IdToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresInSeconds { get; set; }
    }
}
