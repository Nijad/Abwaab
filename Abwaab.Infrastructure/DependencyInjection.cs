using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Identity.Services;
using Abwaab.Infrastructure.Options;
using Abwaab.Infrastructure.Presistence;
using Abwaab.Infrastructure.Presistence.Context;
using Abwaab.Infrastructure.Services;
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

            services.AddDbContext<AppDbContext>(options =>options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

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

            // Register Configuration Options
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.Configure<SmsSettings>(config.GetSection("SmsSettings"));

            // Choose your email implementation:
            // Option 1: SendGrid
            // services.AddTransient<IEmailSender, SendGridEmailSender>();
            // Option 2: SMTP (uncomment to use)
            services.AddTransient<IEmailSender, SmtpEmailSender>();

            // SMS Sender
            //services.AddTransient<ISmsSender, TwilioSmsSender>();

            services.Configure<TextBeeSettings>(
            config.GetSection("TextBeeSettings"));

            // Register HttpClient for TextBee
            services.AddHttpClient<ISmsSender, TextBeeSmsSender>();

            // Orchestrator
            services.AddTransient<IVerificationCodeService, VerificationCodeService>();

            // Memory Cache (for storing verification codes in-memory)
            services.AddMemoryCache();

            // Add HttpClient if required by any services (SendGrid doesn't need it via DI, but good practice)
            services.AddHttpClient();


            return services;
        }

    }
}
