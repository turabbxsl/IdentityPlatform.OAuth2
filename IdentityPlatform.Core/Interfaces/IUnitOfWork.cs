namespace IdentityPlatform.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICustomerRepository Customers { get; }
        IOAuthClientRepository Clients { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IAuthCodeRepository AuthCodes { get; }

        Task<int> SaveChangesAsync();
    }
}
