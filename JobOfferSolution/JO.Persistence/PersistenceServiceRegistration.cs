using JO.Persistence.DataAccess;
using JO.Persistence.Repositories;
using JO.Persistence.Repositories.Contracts;
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
            //services.AddDbContext<JobOfferDbContext>((serviceProvider, options) =>
            //{
            //    var appSettings = serviceProvider.GetRequiredService<IAppSettings>();

            //    var connectionStringName = appSettings.GetConnectionStringName();
            //    var connectionString = configuration.GetConnectionString(connectionStringName);

            //    options.UseSqlServer(connectionString);
            //});
            services.AddDbContextFactory<JobOfferDbContext>((serviceProvider, options) =>
            {
                var appSettings = serviceProvider.GetRequiredService<IAppSettings>();

                var connectionStringName = appSettings.GetConnectionStringName();
                var connectionString = configuration.GetConnectionString(connectionStringName);

                options.UseSqlServer(connectionString);
            });

            //Register Generic Repo
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            //Repos
            services.AddScoped<IJobOfferUsersRepo, JobOfferUsersRepo>();
            services.AddScoped<ICandidateRepo, CandidateRepo>();
            services.AddScoped<IJobOfferTransactionRepo, JobOfferTransactionRepo>();
            services.AddScoped<ITransactionAttachmentRepo, TransactionAttachmentRepo>();

            //Views
            services.AddScoped<IVwJobOfferTransactionRepo, VwJobOfferTransactionRepo>();
            services.AddScoped<IVwTransactionAttachmentRepo, VwTransactionAttachmentRepo>();

            return services;
        }
    }
}
