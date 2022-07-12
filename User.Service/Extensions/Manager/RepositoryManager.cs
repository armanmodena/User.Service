using Microsoft.Extensions.DependencyInjection;
using User.Service.Repositories;
using User.Service.Repositories.Interfaces;

namespace User.Service.Extensions.Manager
{
    public static class RepositoryManager
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserTokenRepository, UserTokenRepository>();

            return services;
        }
    }
}
