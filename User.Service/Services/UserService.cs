using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using User.Service.DTO;
using User.Service.DTO.User;
using User.Service.Models.User;
using User.Service.Repositories.Interfaces;
using User.Service.Services.Interfaces;

namespace User.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly IUserTokenRepository UserTokenRepository;

        public UserService(
          IUserRepository userRepo, 
          IUserTokenRepository userTokenRepository)
        {
            UserRepository = userRepo;
            UserTokenRepository = userTokenRepository;
        }

        public Task<PageResultDto<UserDto>> GetAll(string select, string search, string filterAnd, string filterOr, string filterOut, string orderBy, string direction, int page, int pageSize)
        {
            Dictionary<string, string> filterFields = new Dictionary<string, string>()
            {
                {"id", "id"},
                {"name", "name"},
                {"username", "username"}
            };

            string[] globalSearchFields = { "name", "username" };

            var query = @"select #select# from users #condition# #order# #direction# #perpage# #offset#";

            return UserRepository.GetAllUser(query, select, filterFields, globalSearchFields, search, filterAnd, filterOr, filterOut, orderBy, direction, page, pageSize);
        }

        public async Task<UserModel> Get(int id)
        {
            return await UserRepository.Get(id);
        }

        public async Task<UserModel> GetByUsername(string username)
        {
            return await UserRepository.GetByUsername(username);
        }

        public async Task<UserModel> Insert(UserModel user)
        {
            user.CreatedAt = DateTime.Now;
            user.Id = await UserRepository.Insert(user) ?? 0;
            return user;
        }

        public Task<int> Update(UserModel user)
        {
            user.UpdatedAt = DateTime.Now;
            return UserRepository.Update(user);
        }

        public Task<int> Delete(UserModel user)
        {
            var deleteUser = UserRepository.Delete(user);
            if(deleteUser != null)
            {
                UserTokenRepository.DeleteByUserId(user.Id);
            }
            return deleteUser;
        }

    }
}
