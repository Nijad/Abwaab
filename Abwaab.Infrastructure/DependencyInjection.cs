using Abwaab.Application.Common.Contracts;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Identity;
using Abwaab.Infrastructure.Identity.Services;
using Abwaab.Infrastructure.Presistence;
using Abwaab.Infrastructure.Presistence.Context;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Abwaab.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            return services;

            // Add other infrastructure services here, e.g., email service, file storage service, etc.
        }

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<AppDbContext>(options =>
               options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
            var jwtSettings = config.GetSection(nameof(JwtSettings)).Get<JwtSettings>()
    ?? throw new Exception("JwtSettings are missing in appsettings.json");

            services.AddScoped<IAppDbContextInitializer, AppDbContextInitializer>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ClockSkew = TimeSpan.Zero
                };
            });

            //services.AddMediatR(cfg =>
            //{
            //    // Register all handlers from the Application assembly
            //    cfg.RegisterServicesFromAssembly(Assembly.Load("Abwaab.Application"));

            //    // Add the ValidationPipelineBehavior (runs before handlers)
            //    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            //});

            services.AddValidatorsFromAssembly(Assembly.Load("Abwaab.Application"));

            return services;
        }

    }
}
