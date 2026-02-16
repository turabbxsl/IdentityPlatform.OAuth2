using IdentityPlatform.Core.Interfaces;
using IdentityPlatform.Core.Models.Entities;
using IdentityPlatform.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IdentityPlatform.Infrastructure.Persistence.Repositories
{
    public class RefreshTokenRepository : GenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(PlatformContext context) : base(context) { }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                                 .FirstOrDefaultAsync(c => c.Token == token);
        }

        public Task RevokeAllTokensForCustomerAsync(Guid customerId)
        {
            throw new NotImplementedException();
        }
    }
}
