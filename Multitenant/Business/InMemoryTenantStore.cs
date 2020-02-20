using Multitenant.Business.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Multitenant.Common;
using Multitenant.Repository;
using Repository.Classes;


namespace Multitenant.Business
{
    /// <summary>
    /// In memory store for testing
    /// </summary>
    public class InMemoryTenantStore : ITenantStore<Tenant>
    {
        /// <summary>
        /// Get a tenant for a given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public async Task<Tenant> GetTenantAsync(string identifier)
        {
            var options = new DbContextOptionsBuilder<MultitenantDbContext>()
                .UseInMemoryDatabase(databaseName: "MultitenantDB")
                .Options;
      
            var dbContext = new MultitenantDbContext(options);
            var repository = new MultitenantRepository(dbContext);
            var tenantClients = await repository.GetAll();
            var tenantClient = tenantClients.FirstOrDefault(q => q.ClientKey.ToString() == identifier);
            if (tenantClient != null)
                return new Tenant()
                {
                    DatabaseName = tenantClient.ConnectionString,
                    Identifier = tenantClient.ClientKey.ToString(),
                    Id = tenantClient.ClientId.ToString()
                };


            ////var tenant = new[]
            ////{
            ////    new Tenant{ Id = "80fdb3c0-5888-4295-bf40-ebee0e3cd8f3", Identifier = "localhost" ,DatabaseName = "Database1"},
            ////    new Tenant{ Id = "80fdb3c0-5888-4295-bf40-ebee0e3cd8f5", Identifier = "localhost2" , DatabaseName = "Database2"}
            ////}.SingleOrDefault(t => t.Identifier == identifier);

            //return await Task.FromResult(tenant);
            return null;
        }
    }
}