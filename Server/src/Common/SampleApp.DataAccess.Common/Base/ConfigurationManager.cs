using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace SampleApp.DataAccess.Common
{
    public class ConfigurationManager
    {
        static ConfigurationManager()
        {
            AppSettings = new Dictionary<string, string>();

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);


            var configuration = builder.Build();
   
            AppSettings.Add("uploadFilePath", configuration["uploadFilePath"]);
        }

        public static Dictionary<string, string> AppSettings;

    }



}
