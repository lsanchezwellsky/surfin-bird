using System.Collections.Generic;
using System.Threading.Tasks;
using Repository.Entities;

namespace Repository.Interfaces
{
    public interface ItestRepository
    {
        Task<ICollection<Test>> GetAll();

        Task addTest(Test test);
    }
}