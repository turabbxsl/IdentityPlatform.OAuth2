using IdentityPlatform.Core.Models.Base;

namespace IdentityPlatform.Core.Models.Entities
{
    public class AuthCode:BaseEntity
    {
        public string Code { get; set; }

        public Guid CustomerId { get; set; } 
        public Customer Customer { get; set; } 

        public string ClientId { get; set; } 
        public OAuthClient Client { get; set; }

        public string Scope { get; set; } 
        public DateTime ExpiresAt { get; set; } 
        public bool IsUsed { get; set; } = false;
    }
}
