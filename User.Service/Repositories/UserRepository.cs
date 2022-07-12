using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using User.Service.DBContext;
using User.Service.DTO;
using User.Service.DTO.User;
using User.Service.Extensions;
using User.Service.Models.User;
using User.Service.Repositories.Interfaces;

namespace User.Service.Repositories
{
    public class UserRepository : BaseRepository<UserModel>, IUserRepository
    {
        public UserRepository(IPGSQLContext context) : base(context)
        {

        }

        public Task<PageResult<UserDto>> GetAllUser(string query, string select, Dictionary<string, string> filterFields, string[] globalSearchFields,
           string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize)
        {
            var pagination = new Pagination();

            var genQuery = pagination.genQuery(query, select, filterFields, globalSearchFields, search, filterAnd, filterOut, filterOr, orderBy, direction, page, pageSize);

            var totalCount = DB.Query(genQuery[0]).FirstOrDefault();
            var totalPage = (int)Math.Ceiling((decimal)totalCount.total / pageSize);

            var data = DB.Query<UserDto>(genQuery[1]).ToList();

            var result = new PageResult<UserDto>() { Data = data, Page = page, PageSize = pageSize, TotalCount = totalCount.total, TotalPage = totalPage };
            return Task.FromResult(result);

        }

        public Task<UserModel> GetByUsername(string username)
        {
            return DB.QueryFirstOrDefaultAsync<UserModel>("select * from users where LOWER(username)=@username", new {username});
        }
    }
}
