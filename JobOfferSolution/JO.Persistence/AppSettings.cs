using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace JO.Persistence
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration _configuration;
        private readonly bool isProduction;
        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;

            isProduction = _configuration.GetValue<bool>("Environment:production");
        }

        public bool IsProduction() { return isProduction; }

        public string GetConnectionStringName()
        {
            return isProduction ? "JobOffer" : "JobOfferDev";
        }
    }
}
