using JO.Persistence.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            //Settings and Config
            services.AddSingleton<IAppSettings, AppSettings>();

            // Register EF DbContext using AppSettings
            services.AddDbContextFactory<JobOfferDbContext>((serviceProvider, options) =>
            {
                var appSettings = serviceProvider.GetRequiredService<IAppSettings>();

                var connectionStringName = appSettings.GetConnectionStringName();
                var connectionString = configuration.GetConnectionString(connectionStringName);

                options.UseSqlServer(connectionString);
            });

            return services;
        }
    }
}
