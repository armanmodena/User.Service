using System.Threading.Tasks;
using User.Service.Models.User;

namespace User.Service.Repositories.Interfaces
{
    public interface IUserTokenRepository : IBaseRepository<UserToken>
    {
        Task<UserToken> GetByUserId(int user_id);
        Task<UserToken> GetUserToken(int user_id, string refresh_token);
    }
}
