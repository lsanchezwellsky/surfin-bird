using Microsoft.EntityFrameworkCore;
using Multitenant.Business.Classes.Example;
using Repository.Entities;

namespace Repository
{
    public class Sp5DbContext : DbContext
    {

        private OperationIdService operationIdService;
        public Sp5DbContext(DbContextOptions<Sp5DbContext> options, OperationIdService operationIdService) : base(options)
        {
            this.operationIdService = operationIdService;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var tenantInfo = operationIdService.TenantInfo;
            optionsBuilder.UseInMemoryDatabase(tenantInfo.DatabaseName);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<Test> tests { get; set; }

    }

 
}


