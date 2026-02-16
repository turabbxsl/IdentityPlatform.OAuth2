using IdentityPlatform.Core.Models.Base;

namespace IdentityPlatform.Core.Models.Entities
{
    public class Customer: BaseEntity
    {
        public Guid Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string FullName { get; set; }
        public string IdentityNumber { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
