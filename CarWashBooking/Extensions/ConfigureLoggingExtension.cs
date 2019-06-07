using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarWashBooking.Logging;
using CarWashBooking.Options;
//using Microsoft.Extensions.Options;

namespace CarWashBooking.Extensions
{
    public static class ConfigureLoggingExtension
    {
        public static ILoggingBuilder AddLoggingConfiguration(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
        {
            var loggingOptions = new Options.LoggingOptions();
            configuration.GetSection("Logging").Bind(loggingOptions);

            foreach (var provider in loggingOptions.Providers)
            {
                switch (provider.Name.ToLower())
                {
                    case "console":
                        {
                            loggingBuilder.AddConsole();
                            break;
                        }
                    case "file":
                        {
                            string filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "logs", $"CarWashBooking_{System.DateTime.Now.ToString("ddMMyyHHmm")}.log");
                            loggingBuilder.AddFile(filePath, (LogLevel)provider.LogLevel);
                            string filePathError = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "logs", $"CarWashBooking_Error{System.DateTime.Now.ToString("ddMMyyHHmm")}.log");
                            loggingBuilder.AddFileError (filePathError);

                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }

            return loggingBuilder;
        }
    }
}
