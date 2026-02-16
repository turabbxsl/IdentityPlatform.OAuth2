using IdentityPlatform.Core.Models.Base;

namespace IdentityPlatform.Core.Models.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string Token { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string ClientId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
