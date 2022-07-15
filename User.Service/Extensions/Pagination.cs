using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using User.Service.DBContext;
using User.Service.DTO;

namespace User.Service.Extensions
{
    [Serializable]
    public class Pagination<T>
    {
        public readonly IDbConnection DB = null;

        public Pagination(IPGSQLContext context)
        {
            DB = context.DB;
        }

        public static bool isNumeric(string s)
        {
            return int.TryParse(s, out int n);
        }

        public string setCountQuery(string query, string condition)
        {
            return query.Replace("#select#", "count(1) as total")
                        .Replace("#condition#", condition)
                        .Replace("#order#", "")
                        .Replace("#direction#", "")
                        .Replace("#where#", "")
                        .Replace("#perpage#", "")
                        .Replace("#offset#", "");
        }

        public string setQuery(string query, string select, string[] fields, string condition, string where, 
            string orderBy, string direction, int pageSize, int offset)
        {
            string selectFields = "*";

            if(select != null)
            {
                selectFields = select;
            }
            else if(fields != null)
            {
                selectFields = string.Join(",", fields);
            }

            return query.Replace("#select#", selectFields)
                        .Replace("#condition#", condition)
                        .Replace("#order#", $"order by {orderBy}")
                        .Replace("#direction#", direction)
                        .Replace("#where#", where)
                        .Replace("#perpage#", $"limit {pageSize}")
                        .Replace("#offset#", $"offset {offset}");
        }

        public string setSearch(string[] globalSearch, string search)
        {
            if (string.IsNullOrEmpty(search) || globalSearch.Length == 0)
                return "";

            var query = "";

            int count = 0;
            foreach (var item in globalSearch)
            {
                if (count == 0)
                {
                    query = $" LOWER({item}) like '%{search.ToLower()}%'";
                }
                else
                {
                    query = $" or LOWER({item}) like '%{search.ToLower()}%'";
                }
            }

            return $"where ({query})";
        }

        public string setFilterAnd(string prevCondition, IDictionary filterFields, string filterAnd)
        {
            if (String.IsNullOrEmpty(filterAnd))
                return prevCondition;

            string[] filters = filterAnd.Split(",");

            if (filters.Length == 0)
                return prevCondition;

            var newCondition = "";
            int count = 0;
            foreach (var f in filters)
            {
                string[] value = f.Split(":");
                if (value.Length == 2 && value[1] != null)
                {
                    if (filterFields[value[0]] != null)
                    {
                        if (String.IsNullOrEmpty(prevCondition) && count == 0)
                        {
                            if (isNumeric(value[1]))
                            {
                                newCondition += $"where {filterFields[value[0]]} = {value[1]}";
                            }
                            else
                            {
                                newCondition += $"where LOWER({filterFields[value[0]]}) = '{value[1].ToLower()}'";
                            }
                        }
                        else
                        {
                            if (isNumeric(value[1]))
                            {
                                newCondition += $" and {filterFields[value[0]]} = {value[1]}";
                            }
                            else
                            {
                                newCondition += $" and LOWER({filterFields[value[0]]}) = '{value[1].ToLower()}'";
                            }
                        }
                        count++;
                    }
                }
            }

            prevCondition += newCondition;

            return prevCondition;
        }

        public string setFilterOr(string prevCondition, IDictionary filterFields, string filterOr)
        {
            if (String.IsNullOrEmpty(filterOr))
                return prevCondition;

            string[] filters = filterOr.Split(",");

            if (filters.Length == 0)
                return prevCondition;

            var newCondition = "";
            int count = 0;
            foreach (var f in filters)
            {
                string[] value = f.Split(":");
                if (value.Length == 2 && value[1] != null)
                {
                    if (filterFields[value[0]] != null)
                    {
                        if (String.IsNullOrEmpty(prevCondition) && count == 0)
                        {
                            if (isNumeric(value[1]))
                            {
                                newCondition += $"where {filterFields[value[0]]} = {value[1]}";
                            }
                            else
                            {
                                newCondition += $"where LOWER({filterFields[value[0]]}) = '{value[1].ToLower()}'";
                            }
                        }
                        else
                        {
                            if (isNumeric(value[1]))
                            {
                                newCondition += $" or {filterFields[value[0]]} = {value[1]}";
                            }
                            else
                            {
                                newCondition += $" or LOWER({filterFields[value[0]]}) = '{value[1].ToLower()}'";
                            }
                        }
                        count++;
                    }
                }
            }

            prevCondition += newCondition;

            return prevCondition;
        }

        public string setFilterOut(string select, string[]  fields, string filterOut)
        {
            if (String.IsNullOrEmpty(filterOut))
                return "";

            string[] filters = filterOut.Split(",");

            if (filters.Length == 0)
                return "";

            var where = "";
            int count = 0;
            foreach (var f in filters)
            {
                string[] value = f.Split(":");
                if (value.Length == 2 && value[1] != null)
                {
                    if ((select != null && select.Contains(value[0])) || fields.Any(value[0].Contains))
                    {
                        if (count == 0)
                        {
                            if (isNumeric(value[1]))
                            {
                                where += $"where foo.{value[0]} = {value[1]}";
                            }
                            else
                            {
                                where += $"where LOWER(foo.{value[0]}) = '{value[1].ToLower()}'";
                            }
                        }
                        else
                        {
                            if (isNumeric(value[1]))
                            {
                                where += $" or foo.{value[0]} = {value[1]}";
                            }
                            else
                            {
                                where += $" or LOWER(foo.{value[0]}) = '{value[1].ToLower()}'";
                            }
                        }
                        count++;
                    }
                }
            }

            return where;
        }

        public ErrorDto validation(string select, string[] fields, IDictionary filterFields, string filterAnd, string filterOr, string filterOut)
        {
            if(select != null)
            {
                var selectArray = select.Split(",");
                foreach (var sa in selectArray)
                {
                    if (!fields.Any(sa.Contains))
                    {
                        return new ErrorDto()
                        {
                            code = "422",
                            message = $"Unknown field {sa} on select field"
                        };
                    }
                }
            }

            if(filterAnd != null)
            {
                var filters = filterAnd.Split(",");
                foreach (var fa in filters)
                {
                    string[] value = fa.Split(":");
                    if (filterFields[value[0]] is null)
                    {
                        return new ErrorDto()
                        {
                            code = "422",
                            message = $"Unknown field {fa} on filterAnd"
                        };
                    }
                }
            }

            if (filterOr != null)
            {
                var filters = filterOr.Split(",");
                foreach (var fo in filters)
                {
                    string[] value = fo.Split(":");
                    if (filterFields[value[0]] is null)
                    {
                        return new ErrorDto()
                        {
                            code = "422",
                            message = $"Unknown field {fo} on filterOr"
                        };
                    }
                }
            }

            if (filterOut != null)
            {
                var filters = filterOut.Split(",");
                foreach (var ft in filters)
                {
                    string[] value = ft.Split(":");
                    if (!fields.Any(value[0].Contains))
                    {
                        return new ErrorDto()
                        {
                            code = "422",
                            message = $"Unknown field {value[0]} on filterOut"
                        };
                    }
                }
            }

            return null;
        }


        public string[] genQuery(string query, string select, string[] fields, IDictionary filterFields, string[] globalSearchFields, string search,
            string filterAnd, string filterOut, string filterOr, string orderBy, string direction, int page, int pageSize)
        {
            var condition = "";
            condition = setSearch(globalSearchFields, search);
            condition = setFilterAnd(condition, filterFields, filterAnd);
            condition = setFilterOr(condition, filterFields, filterOr);

            var where = setFilterOut(select, fields, filterOut);

            var countQuery = setCountQuery(query, condition);

            int offset = (page - 1) * pageSize;

            var executeQuery = setQuery(query, select, fields, condition, where, orderBy, direction, pageSize, offset);

            string[] result = { countQuery, executeQuery };

            return result;
        }

        public async Task<(PageResultDto<T>, ErrorDto)> pagination(string query, string select, string[] fields, Dictionary<string, string> filterFields, string[] globalSearchFields,
           string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize)
        {

            var error = validation(select, fields, filterFields, filterAnd, filterOr, filterOut);
            if (error != null)
                return (null, error);

            var setQuery = genQuery(query, select, fields, filterFields, globalSearchFields, search, filterAnd, filterOut, filterOr, orderBy, direction, page, pageSize);

            var totalCount = await DB.QueryFirstOrDefaultAsync(setQuery[0]);
            var totalPage = (int)Math.Ceiling((decimal)totalCount.total / pageSize);

            var data = await DB.QueryAsync<T>(setQuery[1]);

            var result = new PageResultDto<T>() { Data = data, Page = page, PageSize = pageSize, TotalCount = totalCount.total, TotalPage = totalPage };
            return (result, error);
        }
    }
}
