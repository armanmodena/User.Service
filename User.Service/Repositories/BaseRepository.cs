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

        public Task<PageResultDto<T>> GetAll(string query, string select, Dictionary<string, string> filterFields, string[] globalSearchFields,
           string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize)
        {
            var pagination = new Pagination();

            var genQuery = pagination.genQuery(query, select, filterFields, globalSearchFields, search, filterAnd, filterOut, filterOr, orderBy, direction, page, pageSize);

            var totalCount = DB.Query(genQuery[0]).FirstOrDefault();
            var totalPage = (int)Math.Ceiling((decimal)totalCount.total / pageSize);

            var data = DB.Query<T>(genQuery[1]).ToList();

            var result = new PageResultDto<T>() { Data = data, Page = page, PageSize = pageSize, TotalCount = totalCount.total, TotalPage = totalPage };
            return Task.FromResult(result);

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
