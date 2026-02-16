using IdentityPlatform.Core.Models.Entities;

namespace IdentityPlatform.Core.Interfaces
{
    public interface IOAuthClientRepository : IGenericRepository<OAuthClient>
    {
        Task<OAuthClient?> GetByClientIdAsync(string clientId);
    }
}
