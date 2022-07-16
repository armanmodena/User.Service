using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using User.Service.DBContext;
using User.Service.DTO;
using User.Service.Extensions;
using User.Service.Repositories.Interfaces;
using Z.Dapper.Plus;

namespace User.Service.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T>, IDisposable where T : class
    {
        public readonly IDbConnection DB = null;
        public readonly Pagination<T> Pagiantion;

        public BaseRepository(IPGSQLContext context)
        {
            DB = context.DB;
            Pagiantion = new Pagination<T>(context);
        }

        public async Task<(PageResultDto<T>, ErrorDto)> GetAll(string query, string select, string[] fields, Dictionary<string, string> filterFields, string[] globalSearchFields,
           string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize)
        {
            return await Pagiantion.pagination(query, select, fields, filterFields, globalSearchFields, search, filterAnd, filterOr, filterOut, orderBy, direction, page, pageSize);
        }

        public Task<T> Get(int id)
        {
            return DB.GetAsync<T>(id);
        }

        public Task<int?> Insert(T entity)
        {
            return DB.InsertAsync<T>(entity);
        }

        public Task<DapperPlusActionSet<T>> InsertMultiple(List<T> entity)
        {
            return Task.FromResult(DB.BulkInsert<T>(entity));
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
