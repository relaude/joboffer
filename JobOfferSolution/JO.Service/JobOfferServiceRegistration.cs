using JO.Service.Services;
using JO.Service.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Service
{
    public static class JobOfferServiceRegistration
    {
        public static IServiceCollection AddJobOfferServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IManageUsersService, ManageUsersService>();
            services.AddScoped<IUtilitiesService, UtilitiesService>();
            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<ITalentAcquisitionService, TalentAcquisitionService>();

            return services;
        }
    }
}
