
using Abwaab.Application;
using Abwaab.Application.Common.Behaviors;
using Abwaab.Infrastructure;
using Abwaab.Server.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;
using Serilog;

namespace Abwaab.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddProblemDetails();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            // 1. Configure the Serilog Logger
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()               // Global log level
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // Ignore verbose framework logs
                .Enrich.FromLogContext()                  // Add contextual properties
                .WriteTo.Console()                        // Output to Console (optional)
                .WriteTo.File(
                    path: "logs/abwaab-.log",              // File path (logs folder)
                    rollingInterval: RollingInterval.Day, // New log file every day
                    retainedFileCountLimit: 31,           // Keep last 31 days of logs
                    fileSizeLimitBytes: 20 * 1024 * 1024  // Max 20 MB per file
                )
                .CreateLogger();

            // 2. Use Serilog as the host logger
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

            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

            
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // Add MediatR pipeline behaviors
            // ValidationBehavior will run before DetectIdentifierBehavior, as it is registered first
            // This means that validation will occur before identifier detection in the request processing pipeline
            // The order of registration matters because MediatR executes pipeline behaviors in the order they are registered
            // If you want to change the order of execution, you can change the order of registration here
            // For example, if you want DetectIdentifierBehavior to run before ValidationBehavior, you can swap the order of these two lines
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            // DetectIdentifierBehavior will run after ValidationBehavior, as it is registered second
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(DetectIdentifierBehavior<,>));
            // Get user id in a request
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserContextBehavior<,>));

            var app = builder.Build();
            app.UseExceptionHandler();
            
            // 3. Logs all HTTP requests
            app.UseSerilogRequestLogging();

            // Global exception handling (Logs ALL uncaught errors)
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Unhandled exception caught by global middleware.");

                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync("An internal error occurred.");
                }
            });
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            //app.UseMiddleware<GlobalExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                await app.InitialiseDatabaseAsync();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            //app.MapFallbackToFile("/index.html");

            // 4. Catch startup exceptions and flush logs
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
