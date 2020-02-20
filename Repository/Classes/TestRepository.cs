using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Repository.Classes.MDW.openreferrals.Repository;
using Repository.Entities;
using Repository.Interfaces;

namespace Repository.Classes
{
    /// <inheritdoc />
    public sealed class TestRepository : GenericRepository<Test> , ItestRepository

    {
        public TestRepository(Sp5DbContext dbContext) : base(dbContext)
        {
            
        }

        public async Task<ICollection<Test>> GetAll()
        {
            return await Query().ToListAsync();
        }

        public async Task addTest(Test test)
        {
            await InsertAsync(test);
        }
    }
}