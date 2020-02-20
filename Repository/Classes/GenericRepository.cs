using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Repository.Interfaces;

namespace Repository.Classes
{
    // ReSharper disable ConvertToUsingDeclaration

    namespace MDW.openreferrals.Repository
    {
        public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, new()
        {

            protected Sp5DbContext DbContext { get; set; }

            protected GenericRepository(Sp5DbContext dbContext)
            {
                DbContext = dbContext;
            }

            protected GenericRepository()
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

            public async Task<IEnumerable<IDictionary<string, object>>> ExecuteQuery(string query, object[] parameters)
            {
                using (var command = DbContext.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    foreach (var prm in parameters) command.Parameters.Add(prm);
                    DbContext.Database.OpenConnection();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var names = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList<string>();
                        var result = new List<IDictionary<string, object>>();
                        foreach (IDataRecord record in reader as IEnumerable)
                        {
                            IDictionary<string, object> expando;
                            expando = new ExpandoObject() as IDictionary<string, object>;
                            foreach (var name in names)
                                expando[name] = record[name];
                            result.Add(expando);
                        }
                        return result;
                    }


                }


            }


            public async Task ExecuteCommandAdo(string connectionString, string query, object[] parameters = null)
            {
                NpgsqlConnection conn = new NpgsqlConnection(connectionString);
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = query;
                    if (parameters != null)
                        foreach (var prm in parameters)
                            command.Parameters.Add(prm);

                    conn.Open();
                    await command.ExecuteNonQueryAsync();
                }
            }

            public async Task ExecuteCommand(string query)
            {
                await DbContext.Database.ExecuteSqlCommandAsync(query);
            }

            public async Task DeleteAsync(T entity)
            {
                DbContext.Remove(entity);
                await DbContext.SaveChangesAsync();
            }







        }
    }
}