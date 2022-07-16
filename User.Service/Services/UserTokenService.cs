using System.Threading.Tasks;
using User.Service.Models.User;
using User.Service.Repositories.Interfaces;
using User.Service.Services.Interfaces;

namespace User.Service.Services
{
    public class UserTokenService : IUserTokenService
    {
        private readonly IUserTokenRepository UserTokenRepository;

        public UserTokenService(
            IUserTokenRepository userTokenRepo)
        {
            UserTokenRepository = userTokenRepo;
        }

        public async Task<UserToken> GetByUserId(int user_id)
        {
            return await UserTokenRepository.GetByUserId(user_id);
        }

        public async Task<UserToken> GetUserToken(int user_id, string refresh_token)
        {
            return await UserTokenRepository.GetUserToken(user_id, refresh_token);
        }

        public async Task<UserToken> Insert(UserToken userToken)
        {
            userToken.Id = await UserTokenRepository.Insert(userToken) ?? 0;
            return userToken;
        }

        public async Task<int> Update(UserToken userToken)
        {
            return await UserTokenRepository.Update(userToken);
        }
    }
}
