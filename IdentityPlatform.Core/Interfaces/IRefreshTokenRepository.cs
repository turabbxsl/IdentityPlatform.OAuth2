using IdentityPlatform.Core.Models.Entities;

namespace IdentityPlatform.Core.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task RevokeAllTokensForCustomerAsync(Guid customerId);
    }
}
