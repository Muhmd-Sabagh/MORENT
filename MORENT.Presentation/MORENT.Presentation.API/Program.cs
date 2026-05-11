using Microsoft.OpenApi;
using MORENT.Application;
using MORENT.Infrastructure;

namespace Presentation.Server
{
    public class Program
    {
        public async static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // Add services to the container from our Clean Architecture Layers
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // Configure Swagger with JWT Bearer Authentication
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MORENT Car Rental API",
                    Version = "v1",
                    Description = "API for the MORENT Car Rental System"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR...\""
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            // Configure CORS for Angular Frontend (Typically runs on localhost:4200)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularClient",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200")
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();
                    });
            });

            var app = builder.Build();

            // Initialize the database
            await app.InitializeDatabaseAsync();

            app.UseDefaultFiles();
            app.MapStaticAssets();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MORENT API v1"));
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAngularClient");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
