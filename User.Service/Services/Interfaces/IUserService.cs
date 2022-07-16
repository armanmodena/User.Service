using System.Collections.Generic;
using System.Threading.Tasks;
using User.Service.DTO;
using User.Service.DTO.User;
using User.Service.Models.User;
using Z.Dapper.Plus;

namespace User.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<(PageResultDto<UserDto>, ErrorDto)> GetAll(string select, string search, string filterAnd, string filterOr, string filterOut, 
            string orderBy, string direction, int page, int pageSize);

        Task<(PageResultDto<UserWithTokenDto>, ErrorDto)> GetAllWithToken(string select, string search, string filterAnd, string filterOr, 
            string filterOut, string orderBy, string direction, int page, int pageSize);

        Task<UserModel> Get(int id);

        Task<UserModel> GetByUsername(string username);

        Task<UserModel> Insert(UserModel user);

        Task<DapperPlusActionSet<UserModel>> InsertMultiple(List<UserModel> users);

        Task<int> Update(UserModel user);

        Task<int> Delete(UserModel user);
    }
}
