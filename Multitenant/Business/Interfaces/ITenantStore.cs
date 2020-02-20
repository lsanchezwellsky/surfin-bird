using System.Threading.Tasks;


namespace Multitenant.Business.Interfaces
{
    public interface ITenantStore<T>
    {
        Task<T> GetTenantAsync(string identifier);
    }
}