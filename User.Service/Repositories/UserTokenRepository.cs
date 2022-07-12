using System.Threading.Tasks;
using Dapper;
using User.Service.DBContext;
using User.Service.Models.User;
using User.Service.Repositories.Interfaces;

namespace User.Service.Repositories
{
    public class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(IPGSQLContext context) : base(context)
        {

        }
        public async Task<UserToken> GetByUserId(int user_id)
        {
            return await DB.QueryFirstOrDefaultAsync<UserToken>("select * from user_token where user_id=@user_id", new { user_id });
        }

        public async Task<UserToken> GetUserToken(int user_id, string refresh_token)
        {
            return await DB.QueryFirstOrDefaultAsync<UserToken>("select * from user_token where user_id=@user_id and refresh_token=@refresh_token", new { user_id, refresh_token });
        }

        public async Task<UserToken> DeleteByUserId(int user_id)
        {
            return await DB.QueryFirstOrDefaultAsync<UserToken>("delete from user_token where user_id=@user_id", new { user_id });

        }
    }
}
