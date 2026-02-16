using IdentityPlatform.Core.Interfaces;
using IdentityPlatform.Core.Models.Entities;
using IdentityPlatform.Infrastructure.Persistence.Context;

namespace IdentityPlatform.Infrastructure.Persistence.Repositories
{
    public class AuthCodeRepository:GenericRepository<AuthCode>,IAuthCodeRepository
    {

        public AuthCodeRepository(PlatformContext context) : base(context) { }

    }
}
