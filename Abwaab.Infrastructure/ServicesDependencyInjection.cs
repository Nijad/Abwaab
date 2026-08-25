using Abwaab.Application.Contracts;
using Abwaab.Application.Contracts.Properties;
using Abwaab.Application.Interfaces;
using Abwaab.Infrastructure.Services;
using Abwaab.Infrastructure.Services.PropertyServices;
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
            services.AddScoped<IUserPlanStateService, UserPlanStateService>();
            services.AddScoped<IPropertyService, PropertyService>();
            services.AddScoped<IPropertyStatesService, PropertyStatesService>();
            services.AddScoped<IPropertyTypeService, PropertyTypeService>();
            services.AddScoped<IPropertyFinishingService, PropertyFinishingService>();
            services.AddScoped<IPropertyTimeSlotService, PropertyTimeSlotService>();
            services.AddScoped<IPropertyAttributeService, PropertyAttributeService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IJwtService, JwtService>();
            return services;
        }
    }
}
