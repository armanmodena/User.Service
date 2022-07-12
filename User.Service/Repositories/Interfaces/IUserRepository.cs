using System.Collections.Generic;
using System.Threading.Tasks;
using User.Service.DTO;
using User.Service.DTO.User;
using User.Service.Models.User;

namespace User.Service.Repositories.Interfaces
{
    public interface IUserRepository : IBaseRepository<UserModel>
    {
        public Task<PageResult<UserDto>> GetAllUser(string query, string select, Dictionary<string, string> filterFields, string[] globalSearchFields,
          string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize);

        Task<UserModel> GetByUsername(string username);
    }
}
