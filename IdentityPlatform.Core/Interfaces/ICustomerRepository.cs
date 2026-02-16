using IdentityPlatform.Core.Models.Entities;

namespace IdentityPlatform.Core.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<Customer?> GetByRegistrationNumberAsync(string registrationNumber);
    }
}
