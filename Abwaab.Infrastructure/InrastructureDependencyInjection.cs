using Abwaab.Application.Common.Contracts;
using Abwaab.Application.Common.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Options;
using Abwaab.Infrastructure.Presistence;
using Abwaab.Infrastructure.Presistence.Context;
using Abwaab.Infrastructure.Services.Common;
using Abwaab.Infrastructure.Services.EmailServices;
using Abwaab.Infrastructure.Services.SmsServices;
using Abwaab.Infrastructure.Services.UserServices;
using FluentValidation;
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
    public static class InrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
            var jwtSettings = config.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ?? throw new Exception("JwtSettings are missing in appsettings.json");

            // Register IUserContext as scoped (per request)
            services.AddScoped<IUserContext, UserContext>();

            services.AddScoped<IAppDbContextInitializer, AppDbContextInitializer>();

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings?.Issuer,
                    ValidAudience = jwtSettings?.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret))
                };
            });

            services.AddAuthorization();

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

            services.AddTransient<IVerificationCodeService, VerificationCodeDemoService>();

            // Memory Cache (for storing verification codes in-memory)
            services.AddMemoryCache();

            // Add HttpClient if required by any services (SendGrid doesn't need it via DI, but good practice)
            services.AddHttpClient();

            services.AddHttpContextAccessor();
            services.AddScoped<IUrlBuilder, UrlBuilder>();

            return services;
        }

    }
}
