using System.Threading.Tasks;
using User.Service.Models.User;

namespace User.Service.Services.Interfaces
{
    public interface IUserTokenService
    {

        Task<UserToken> GetByUserId(int user_id);

        Task<UserToken> GetUserToken(int user_id, string refresh_token);

        Task<UserToken> Insert(UserToken userToken);

        Task<int> Update(UserToken userToken);
    }
}
