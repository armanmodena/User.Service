using System;
using System.Collections.Generic;
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

        public async Task<(PageResultDto<UserDto>, ErrorDto)> GetAllUser(string query, string select, string[] fields, Dictionary<string, string> filterFields, string[] globalSearchFields,
           string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize)
        {
            var pagination = new Pagination();

            var error = pagination.validation(select, fields, filterFields, filterAnd, filterOr, filterOut);
            if (error != null)
                return (null, error);

            var genQuery = pagination.genQuery(query, select, fields, filterFields, globalSearchFields, search, filterAnd, filterOut, filterOr, orderBy, direction, page, pageSize);

            var totalCount = await DB.QueryFirstOrDefaultAsync(genQuery[0]);
            var totalPage = (int)Math.Ceiling((decimal)totalCount.total / pageSize);

            var data = await DB.QueryAsync<UserDto>(genQuery[1]);

            var result = new PageResultDto<UserDto>() { Data = data, Page = page, PageSize = pageSize, TotalCount = totalCount.total, TotalPage = totalPage };
            return (result, null);

        }

        public async Task<(PageResultDto<UserWithTokenDto>, ErrorDto)> GetAllUserWithToken(string query, string select, string[] fields, Dictionary<string, string> filterFields, string[] globalSearchFields,
           string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize)
        {
            var pagination = new Pagination();

            var error = pagination.validation(select, fields, filterFields, filterAnd, filterOr, filterOut);
            if (error != null)
                return (null, error);

            var genQuery = pagination.genQuery(query, select, fields, filterFields, globalSearchFields, search, filterAnd, filterOut, filterOr, orderBy, direction, page, pageSize);

            var totalCount = await DB.QueryFirstOrDefaultAsync(genQuery[0]);
            var totalPage = (int)Math.Ceiling((decimal)totalCount.total / pageSize);

            var data = await DB.QueryAsync<UserWithTokenDto>(genQuery[1]);

            var result = new PageResultDto<UserWithTokenDto>() { Data = data, Page = page, PageSize = pageSize, TotalCount = totalCount.total, TotalPage = totalPage };
            return (result, null);
        }


        public Task<UserModel> GetByUsername(string username)
        {
            return DB.QueryFirstOrDefaultAsync<UserModel>("select * from users where LOWER(username)=@username", new {username});
        }
    }
}
