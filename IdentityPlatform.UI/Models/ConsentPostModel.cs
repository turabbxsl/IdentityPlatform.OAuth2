using IdentityPlatform.UI.Models.Enums;

namespace IdentityPlatform.UI.Models
{
    public class ConsentPostModel
    {
        public string Code { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public ConsentDecision Decision { get; set; }
    }
}
