using System;
using System.Collections;

namespace User.Service.Extensions
{
    [Serializable]
    public class Pagination
    {
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

        public string setQuery(string query, string select, string condition, string orderBy, string direction, int pageSize, int offset, string where)
        {
            return query.Replace("#select#", !String.IsNullOrEmpty(select) ? select : "*")
                        .Replace("#condition#", condition)
                        .Replace("#order#", "order by " + orderBy)
                        .Replace("#direction#", direction)
                        .Replace("#where#", where)
                        .Replace("#perpage#", "limit " + pageSize)
                        .Replace("#offset#", "offset " + offset);
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
                    query = " LOWER(" + item + ") like '%" + search.ToLower() + "%'";
                }
                else
                {
                    query = " or LOWER(" + item + ") like '%" + search.ToLower() + "%'";
                }
            }

            return "where (" + query + ")";
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
                                newCondition += "where " + filterFields[value[0]] + " = " + value[1];
                            }
                            else
                            {
                                newCondition += "where LOWER(" + filterFields[value[0]] + ") = '" + value[1].ToLower() + "'";
                            }
                        }
                        else
                        {
                            if (isNumeric(value[1]))
                            {
                                newCondition += " and " + filterFields[value[0]] + " = " + value[1];
                            }
                            else
                            {
                                newCondition += " and LOWER(" + filterFields[value[0]] + ") = '" + value[1].ToLower() + "'";
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
                                newCondition += "where " + filterFields[value[0]] + " = " + value[1];
                            }
                            else
                            {
                                newCondition += "where LOWER(" + filterFields[value[0]] + ") = '" + value[1].ToLower() + "'";
                            }
                        }
                        else
                        {
                            if (isNumeric(value[1]))
                            {
                                newCondition += " or " + filterFields[value[0]] + " = " + value[1];
                            }
                            else
                            {
                                newCondition += " or LOWER(" + filterFields[value[0]] + ") = '" + value[1].ToLower() + "'";
                            }
                        }
                        count++;
                    }
                }
            }

            prevCondition += newCondition;

            return prevCondition;
        }

        public string setFilterOut(string fields, string filterOut)
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
                    if (fields.Contains(value[0]))
                    {
                        if (count == 0)
                        {
                            if (isNumeric(value[1]))
                            {
                                where += "where " + value[0] + " = " + value[1];
                            }
                            else
                            {
                                where += "where LOWER(" + value[0] + ") = '" + value[1].ToLower() + "'";
                            }
                        }
                        else
                        {
                            if (isNumeric(value[1]))
                            {
                                where += " or " + value[0] + " = " + value[1];
                            }
                            else
                            {
                                where += " or LOWER(" + value[0] + ") = '" + value[1].ToLower() + "'";
                            }
                        }
                        count++;
                    }
                }
            }

            return where;
        }

        public string[] genQuery(string query, string select, IDictionary filterFields, string[] globalSearchFields, string search,
            string filterAnd, string filterOut, string filterOr, string orderBy, string direction, int page, int pageSize)
        {
            var condition = "";
            condition = setSearch(globalSearchFields, search);
            condition = setFilterAnd(condition, filterFields, filterAnd);
            condition = setFilterOr(condition, filterFields, filterOr);

            var where = setFilterOut(select, filterOut);

            var countQuery = setCountQuery(query, condition);

            int offset = (page - 1) * pageSize;

            var executeQuery = setQuery(query, select, condition, orderBy, direction, pageSize, offset, where);

            string[] result = { countQuery, executeQuery };

            return result;
        }
    }
}
