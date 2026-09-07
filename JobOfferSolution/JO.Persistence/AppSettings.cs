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
        private readonly bool isLocal;
        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;

            isProduction = _configuration.GetValue<bool>("Environment:production");
            isLocal = _configuration.GetValue<bool>("Environment:local");
        }

        public bool IsProduction() { return isProduction; }

        public string GetConnectionStringName()
        {
            return isProduction ? "JobOffer" : "JobOfferDev";
        }

        public string GetOneDriveLocation()
        {
            string local = _configuration.GetValue<string>("OneDrive:local");
            string dev = _configuration.GetValue<string>("OneDrive:dev");
            string prod = _configuration.GetValue<string>("OneDrive:prod");
            
            return isProduction ? prod : (isLocal ? local : dev);
        }
    }
}
