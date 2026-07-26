using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Infrastructure.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;

namespace Abwaab.Infrastructure
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            //services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
