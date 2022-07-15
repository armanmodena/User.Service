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
        public readonly Pagination<UserDto> PaginationUserDto;
        public readonly Pagination<UserWithTokenDto> PaginationUserWithTokenDto;
        public UserRepository(IPGSQLContext context) : base(context)
        {
            PaginationUserDto = new Pagination<UserDto>(context);
            PaginationUserWithTokenDto = new Pagination<UserWithTokenDto>(context);
        }

        public async Task<(PageResultDto<UserDto>, ErrorDto)> GetAllUser(string query, string select, string[] fields, Dictionary<string, string> filterFields,
            string[] globalSearchFields, string search, string filterAnd, string filterOr, string filterOut,
            string orderBy, string direction, int page, int pageSize)
        {
            return await PaginationUserDto.pagination(query, select, fields, filterFields, globalSearchFields, search, filterAnd, filterOr, filterOut, 
                orderBy, direction, page, pageSize);
        }

        public async Task<(PageResultDto<UserWithTokenDto>, ErrorDto)> GetAllUserWithToken(string query, string select, string[] fields, Dictionary<string, string> filterFields,
            string[] globalSearchFields, string search, string filterAnd, string filterOr, string filterOut,
            string orderBy, string direction, int page, int pageSize)
        {
            return await PaginationUserWithTokenDto.pagination(query, select, fields, filterFields, globalSearchFields, search, filterAnd, filterOr, 
                filterOut, orderBy, direction, page, pageSize);
        }

        public Task<UserModel> GetByUsername(string username)
        {
            return DB.QueryFirstOrDefaultAsync<UserModel>("select * from users where LOWER(username)=@username", new {username});
        }
    }
}
