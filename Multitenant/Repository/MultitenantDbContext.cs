using System;
using Microsoft.EntityFrameworkCore;
using Multitenant.Business.Classes.Example;
using Multitenant.Common.Multitenant;

namespace Multitenant.Repository
{
    public class MultitenantDbContext : DbContext
    {

        
        public MultitenantDbContext(DbContextOptions<MultitenantDbContext> options) : base(options)
        {
         
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
            optionsBuilder.UseInMemoryDatabase("MultitenantDB");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }

        public DbSet<MultitenantClient> MultitenantClients { get; set; }

    }
 
    
}