
namespace Multitenant.Business.Interfaces
{
    public interface ITenantAccessor<T>
    {
        T Tenant { get; }
    }
}