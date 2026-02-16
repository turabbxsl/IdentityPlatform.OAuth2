using IdentityPlatform.Core.Models.Base;

namespace IdentityPlatform.Core.Models.Entities
{
    public class OAuthClient : BaseEntity
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ClientName { get; set; }
        public string RedirectUri { get; set; }
        public string AllowedScopes { get; set; }
    }
}
