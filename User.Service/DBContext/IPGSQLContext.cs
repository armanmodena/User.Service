using System.Data;

namespace User.Service.DBContext
{
    public interface IPGSQLContext
    {
        IDbConnection DB { get; }
    }
}
