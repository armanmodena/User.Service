using System.Data;
using System.Linq;
using Dapper;

namespace User.Service.Extensions
{
    public class ModelValidator<T> where T : class
    {
        public static bool isNumeric(string s)
        {
            return int.TryParse(s, out int n);
        }

        public bool NotContainSpace(string value)
        {

            return value.Contains(' ') ? false : true;
        }

        public bool IsUnique(IDbConnection DB, string column, string value)
        {
            var newValue = value.ToLower();
            var data = DB.GetList<T>("where LOWER(" + column + ")=@newValue", new { newValue }).FirstOrDefault();
            return data is null ? true : false;
        }

        public bool IsUniqueUpdate(IDbConnection DB, string column, string value, int id)
        {
            var newValue = value.ToLower();
            var data = DB.GetList<T>("where LOWER(" + column + ")=@newValue and id!=@id", new { newValue, id }).FirstOrDefault();
            return data is null ? true : false;
        }

        public bool IsDataExist(IDbConnection DB, string column, string value)
        {
            if(isNumeric(value))
            {
                int.TryParse(value, out int newValue);
                var data = DB.GetList<T>("where " + column + "=@newValue", new { newValue }).FirstOrDefault();
                return data is null ? false : true;
            }
            else
            {
                var newValue = value.ToLower();
                var data = DB.GetList<T>("where LOWER(" + column + ")=@newValue", new { newValue }).FirstOrDefault();
                return data is null ? false : true;
            }

        }
    }
}
