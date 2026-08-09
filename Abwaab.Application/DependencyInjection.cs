using Abwaab.Application.Common.Behaviors;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Abwaab.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {
            // 1. Register MediatR and scan the current assembly (Application)
            services.AddMediatR(cfg =>
            {
                // Scan the Application assembly (where this code is running)
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // 2. Add MediatR pipeline behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // ValidationBehavior will run before DetectIdentifierBehavior, as it is registered first
            // This means that validation will occur before identifier detection in the request processing pipeline
            // The order of registration matters because MediatR executes pipeline behaviors in the order they are registered
            // If you want to change the order of execution, you can change the order of registration here
            // For example, if you want DetectIdentifierBehavior to run before ValidationBehavior, you can swap the order of these two lines
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // DetectIdentifierBehavior will run after ValidationBehavior, as it is registered second
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DetectIdentifierBehavior<,>));
            // Get user id in a request
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserContextBehavior<,>));

            // 3. Register FluentValidation Validators (scan the Application assembly)
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
