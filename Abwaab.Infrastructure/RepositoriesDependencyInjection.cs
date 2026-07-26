using Abwaab.Application.Common.Interfaces;
using Abwaab.Infrastructure.Presistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Abwaab.Infrastructure
{
    public static class RepositoriesDependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<INotificationWayRepository, NotificationWayRepository>();
            return services;
        }
    }
}
