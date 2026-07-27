
using Abwaab.Application;
using Abwaab.Infrastructure;
using Abwaab.Server.Behaviors;
using Abwaab.Server.Exceptions;
using FluentValidation;
using MediatR;
using Microsoft.OpenApi.Models;

namespace Abwaab.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddProblemDetails();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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
            //app.UseDefaultFiles();
            //app.UseStaticFiles();
            //app.UseMiddleware<GlobalExceptionMiddleware>();
            if (app.Environment.IsDevelopment())
            {
                await app.InitialiseDatabaseAsync();
                app.UseSwagger();
                app.UseSwaggerUI(/*c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Abwaab API V1");
                }*/);
            }
            app.UseRateLimiter();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            //app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
