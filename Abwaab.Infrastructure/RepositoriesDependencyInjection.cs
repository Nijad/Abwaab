using Abwaab.Application.Repositories;
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
            services.AddScoped<IPlanRepository, PlanRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IPropertyRepository, PropertyRepository>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            services.AddScoped<IAttributeRepository, AttributeRepository>();
            return services;
        }
    }
}
