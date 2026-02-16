using IdentityPlatform.Core.Interfaces;
using IdentityPlatform.Infrastructure.Persistence.Context;
using IdentityPlatform.Infrastructure.Persistence.Repositories;

namespace IdentityPlatform.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PlatformContext _context;
        private ICustomerRepository _customerRepository;
        private IOAuthClientRepository _clientRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private IAuthCodeRepository _authCodeRepository;

        public UnitOfWork(PlatformContext context)
        {
            _context = context;
        }

        public ICustomerRepository Customers => _customerRepository ??= new CustomerRepository(_context);
        public IOAuthClientRepository Clients => _clientRepository ??= new OAuthClientRepository(_context);
        public IRefreshTokenRepository RefreshTokens => _refreshTokenRepository ??= new RefreshTokenRepository(_context);
        public IAuthCodeRepository AuthCodes => _authCodeRepository ??= new AuthCodeRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
