using Microsoft.Extensions.DependencyInjection;
using User.Service.DBContext;
using User.Service.Services;
using User.Service.Services.Interfaces;

namespace User.Service.Extensions.Manager
{
    public static class ServiceManager
    {
        public static IServiceCollection RegisterService(this IServiceCollection services)
        {
            services.AddScoped<IPGSQLContext, PGSQLContext>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserTokenService, UserTokenService>();

            return services;
        }
    }
}
