using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MORENT.Application.Common;
using MORENT.Application.Interfaces.Auth;
using MORENT.Application.Interfaces.Persistence;
using MORENT.Infrastructure.AuthServices;
using MORENT.Infrastructure.Persistence;
using MORENT.Infrastructure.Repositories;
using System.Text;

namespace MORENT.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. Register DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // 2. Register Repositories & UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 3. Register Auth Service & Configure JWT
            services.AddScoped<IAuthService, AuthService>();
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

            // 4. Check Device environment variables first. Fallback to appsettings only in local dev.
            var secureKeyString = Environment.GetEnvironmentVariable("MORENT_JWT_SECRET") ?? jwtSettings!.Key;
            var key = Encoding.ASCII.GetBytes(secureKeyString);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // true in Production
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings!.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // ADD THIS DEBUGGING BLOCK:
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // This will print the exact reason to your console!
                        Console.WriteLine("=== TOKEN VALIDATION FAILED ===");
                        Console.WriteLine($"Reason: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        Console.WriteLine($"=== CHALLENGE ISSUED ===");
                        Console.WriteLine($"Error: {context.Error}, Description: {context.ErrorDescription}");
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        // 5. Initialize the database with seed data
        public static async Task InitializeDatabaseAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await DatabaseSeeder.SeedAsync(context);
        }
    }
}