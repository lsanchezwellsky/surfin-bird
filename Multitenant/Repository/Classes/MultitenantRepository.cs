using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Multitenant.Common.Multitenant;
using Multitenant.Repository;
using Repository.Classes.MDW.openreferrals.Repository;
using Repository.Interfaces;

namespace Repository.Classes
{
    public class MultitenantRepository : TenantTenantGenericRepository<MultitenantClient>, IMultitenantRepository
    {
        public MultitenantRepository(MultitenantDbContext dbContext) : base(dbContext)
        {
        }


        public async Task<ICollection<MultitenantClient>> GetAll()
        {
            return await Query().ToListAsync();
        }

        public async Task Add(MultitenantClient tenant)
        {
            await InsertAsync(tenant);
        }
    }
}