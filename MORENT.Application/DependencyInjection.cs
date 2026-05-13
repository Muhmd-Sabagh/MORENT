using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MORENT.Application.Interfaces.Services;
using MORENT.Application.Services;

namespace MORENT.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register AutoMapper scanning the current assembly for MappingProfile
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());

            // Register Application Services
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<IRentalService, RentalService>();

            return services;
        }
    }
}