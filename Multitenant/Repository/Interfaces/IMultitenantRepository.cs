using System.Collections.Generic;
using System.Threading.Tasks;
using Multitenant.Common.Multitenant;

namespace Repository.Interfaces
{
    public interface IMultitenantRepository
    {
        Task<ICollection<MultitenantClient>> GetAll();
    }
}