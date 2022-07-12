using System.Collections.Generic;
using System.Threading.Tasks;
using User.Service.DTO;

namespace User.Service.Repositories.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<PageResultDto<T>> GetAll(string query, string select, Dictionary<string, string> filterFields, string[] globalSearchFields,
            string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize);

        Task<T> Get(int id);

        Task<int?> Insert(T entity);

        Task<int> Update(T entity);

        Task<int> Delete(T entity);
    }
}
