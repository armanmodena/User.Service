using System.Threading.Tasks;
using User.Service.DTO;
using User.Service.DTO.User;
using User.Service.Models.User;

namespace User.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<PageResult<UserDto>> GetAll(string select, string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize);

        Task<UserModel> Get(int id);

        Task<UserModel> GetByUsername(string username);

        Task<UserModel> Insert(UserModel user);

        Task<int> Update(UserModel user);

        Task<int> Delete(UserModel user);
    }
}
