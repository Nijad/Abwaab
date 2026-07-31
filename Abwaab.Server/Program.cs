using Abwaab.Application;
using Abwaab.Infrastructure;
using Abwaab.Server.Exceptions;
using Abwaab.Server.Extensions;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

namespace Abwaab.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddProblemDetails();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning() // Global log level
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Ignore verbose framework logs
                .Enrich.FromLogContext() // Add contextual properties
                .WriteTo.Console()  // Output to Console (optional)
                .WriteTo.File(
                    path: "logs/abwaab-.log",              // File path (logs folder)
                    rollingInterval: RollingInterval.Day, // New log file every day
                    retainedFileCountLimit: 31,           // Keep last 31 days of logs
                    fileSizeLimitBytes: 20 * 1024 * 1024  // Max 20 MB per file
                )
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "YourApp API", Version = "v1" });

                // Add JWT authentication button to Swagger UI
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddRepositories();
            builder.Services.AddServices();

            builder.Services.AddApplication(builder.Configuration);

            builder.Services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(Program).Assembly));

            var app = builder.Build();
            app.UseExceptionHandler();

            app.UseSerilogRequestLogging();

            app.UseExceptionHandler();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //app.UseMiddleware<GlobalExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                await app.InitialiseDatabaseAsync();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("/index.html");
            });

            try
            {
                Log.Information("Application is starting up.");
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush(); // Ensure logs are written before exit
            }
        }
    }
}
