using IdentityPlatform.Core.Interfaces;
using IdentityPlatform.Core.Models.Entities;
using IdentityPlatform.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace IdentityPlatform.Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(PlatformContext context) : base(context) { }

        public async Task<Customer?> GetByRegistrationNumberAsync(string registrationNumber)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(c => c.RegistrationNumber == registrationNumber);
        }
    }
}
