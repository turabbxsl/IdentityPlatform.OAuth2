using IdentityPlatform.Core.Interfaces;
using IdentityPlatform.Core.Models.Entities;
using IdentityPlatform.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IdentityPlatform.Infrastructure.Persistence.Repositories
{
    public class OAuthClientRepository : GenericRepository<OAuthClient>, IOAuthClientRepository
    {
        public OAuthClientRepository(PlatformContext context) : base(context) { }

        public async Task<OAuthClient?> GetByClientIdAsync(string clientId)
        {
            return await _context.Clients
                                 .FirstOrDefaultAsync(c => c.ClientId == clientId);
        }
    }
}
