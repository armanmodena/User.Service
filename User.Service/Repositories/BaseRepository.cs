using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using User.Service.DBContext;
using User.Service.DTO;
using User.Service.Extensions;
using User.Service.Repositories.Interfaces;

namespace User.Service.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
    {
        public readonly IDbConnection DB = null;

        public BaseRepository(IPGSQLContext context)
        {
            DB = context.DB;
        }

        public async Task<(PageResultDto<T>, ErrorDto)> GetAll(string query, string select, string[] fields, Dictionary<string, string> filterFields, string[] globalSearchFields,
           string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize)
        {
            var pagination = new Pagination();

            var error = pagination.validation(select, fields, filterFields, filterAnd, filterOr, filterOut);
            if (error != null)
                return (null, error);

            var genQuery = pagination.genQuery(query, select, fields, filterFields, globalSearchFields, search, filterAnd, filterOut, filterOr, orderBy, direction, page, pageSize);

            var totalCount = await DB.QueryFirstOrDefaultAsync(genQuery[0]);
            var totalPage = (int)Math.Ceiling((decimal)totalCount.total / pageSize);

            var data = await DB.QueryAsync<T>(genQuery[1]);

            var result = new PageResultDto<T>() { Data = data, Page = page, PageSize = pageSize, TotalCount = totalCount.total, TotalPage = totalPage };
            return (result, error);

        }

        public Task<T> Get(int id)
        {
            return DB.GetAsync<T>(id);
        }

        public Task<int?> Insert(T entity)
        {
            return DB.InsertAsync<T>(entity);
        }

        public Task<int> Update(T entity)
        {
            return DB.UpdateAsync<T>(entity);
        }

        public Task<int> Delete(T entity)
        {
            return DB.DeleteAsync<T>(entity);
        }

        public void Dispose()
        {
            
        }
    }
}
