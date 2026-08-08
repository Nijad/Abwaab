using Abwaab.Application.Contracts;
using Abwaab.Application.Interfaces;
using Abwaab.Infrastructure.Services;
using Abwaab.Infrastructure.Services.UserServices;
using Microsoft.Extensions.DependencyInjection;

namespace Abwaab.Infrastructure
{
    public static class ServicesDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
