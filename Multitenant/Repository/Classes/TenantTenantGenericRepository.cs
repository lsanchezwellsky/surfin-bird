using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Multitenant.Repository;

using Repository.Interfaces;

namespace Repository.Classes
{
    // ReSharper disable ConvertToUsingDeclaration

    namespace MDW.openreferrals.Repository
    {
        public abstract class TenantTenantGenericRepository<T> : ITenantGenericRepository<T> where T : class, new()
        {

            protected MultitenantDbContext DbContext { get; set; }

            protected TenantTenantGenericRepository(MultitenantDbContext dbContext)
            {
                DbContext = dbContext;
            }

            protected TenantTenantGenericRepository()
            {
            }

            public Task<List<T>> GetAllRecords()
            {
                return DbContext.Set<T>().ToListAsync();
            }

            public async Task<T> GetAsync(int id)
            {
                return await DbContext.FindAsync<T>(id);
            }

            public IQueryable<T> Query()
            {
                return DbContext.Set<T>();
            }


            public async Task InsertAsync(T entity)
            {
                DbContext.Set<T>().Add(entity);
                await DbContext.SaveChangesAsync();
            }

            public async Task UpdateAsync(T entity)
            {
                DbContext.Entry(entity).State = EntityState.Modified;
                await DbContext.SaveChangesAsync();
            }

            public async Task DeleteAsync(int[] ids)
            {
                foreach (var id in ids)
                {
                    var entity = DbContext.FindAsync<T>(id);
                    DbContext.Remove(entity);
                }
                await DbContext.SaveChangesAsync();
            }

            public Task<IEnumerable<IDictionary<string, object>>> ExecuteQuery(string query, object[] parameters)
            {
                throw new System.NotImplementedException();
            }

            public async Task DeleteAsync(T entity)
            {
                DbContext.Remove(entity);
                await DbContext.SaveChangesAsync();
            }







        }
    }
}