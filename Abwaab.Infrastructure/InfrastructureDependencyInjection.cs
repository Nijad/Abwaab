using Abwaab.Application.Interfaces;
using Abwaab.Domain.Entities.UserEntities;
using Abwaab.Infrastructure.Options;
using Abwaab.Infrastructure.Presistence;
using Abwaab.Infrastructure.Presistence.Context;
using Abwaab.Infrastructure.Presistence.Seeding;
using Abwaab.Infrastructure.Services.Common;
using Abwaab.Infrastructure.Services.EmailServices;
using Abwaab.Infrastructure.Services.Notifications;
using Abwaab.Infrastructure.Services.SmsServices;
using Abwaab.Infrastructure.Services.StorageServices;
using Abwaab.Infrastructure.Services.UserServices;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

namespace Abwaab.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddMemoryCache();
            services.AddScoped<ITokenCacheService, TokenCacheService>();
            services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
            var jwtSettings = config.GetSection(nameof(JwtSettings)).Get<JwtSettings>() ?? throw new Exception("JwtSettings are missing in appsettings.json");

            // Register IUserContext as scoped (per request)
            services.AddScoped<IUserContext, UserContext>();

            services.AddScoped<IAppDbContextInitializer, AppDbContextInitializer>();

            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("LoginPolicy", opt =>
                {
                    opt.PermitLimit = 5;
                    opt.Window = TimeSpan.FromMinutes(5);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 0;
                });
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            })
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var jti = context.Principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                        if (!string.IsNullOrEmpty(jti))
                        {
                            var blacklistService = context.HttpContext.RequestServices
                                .GetRequiredService<ITokenCacheService>();
                            if (blacklistService.IsBlacklisted(jti))
                            {
                                context.Fail("Token has been revoked.");
                            }
                        }
                        return Task.CompletedTask;
                    }
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

            //services.AddTransient<IVerificationCodeService, VerificationCodeDemoService>();
            services.AddTransient<IVerificationCodeService, VerificationCodeService>();

            // Memory Cache (for storing verification codes in-memory)
            services.AddMemoryCache();

            // Add HttpClient if required by any services (SendGrid doesn't need it via DI, but good practice)
            services.AddHttpClient();

            services.AddHttpContextAccessor();
            services.AddScoped<IUrlBuilder, UrlBuilder>();
            services.AddScoped<IStorageService, LocalStorageService>();
            services.AddScoped<ITransactionManager, EfCoreTransactionManager>();
            services.AddScoped<INotificationChannel, EmailChannel>();
            services.AddScoped<INotificationChannel, SmsChannel>();

            return services;
        }

    }
}
