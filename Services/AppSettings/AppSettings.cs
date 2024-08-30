using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HFPMapp.Services.AppSettings
{
    public class AppSettings
    {
        public string ApiBaseUrl { get; set; }
        public string EncryptionKey { get; set; }

        public AppSettings(IConfiguration configuration)
        {
            ApiBaseUrl = configuration["ApiBaseUrl"];
            EncryptionKey = configuration["EncryptionKey"];
        }

    }
}
