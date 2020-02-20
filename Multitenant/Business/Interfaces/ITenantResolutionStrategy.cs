using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Multitenant.Repository.Interfaces
{
    public interface ITenantResolutionStrategy
    {
        Task<string> GetTenantIdentifierAsync();
    }
}