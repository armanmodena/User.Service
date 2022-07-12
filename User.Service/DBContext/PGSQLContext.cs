using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace User.Service.DBContext
{
    public class PGSQLContext : IPGSQLContext
    {
        public IDbConnection DB { get; private set; }

        public PGSQLContext(IConfiguration config)
        {
            DB = new NpgsqlConnection(config.GetConnectionString("Data"));
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
        }
    }
}
