using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarWashBooking.Options
{
    public class LoggingOptions
    {
        public LoggingProviderOption[] Providers { get; set; }
    }
    public class LoggingProviderOption
    {
        public string Name { get; set; }
        public string Parameters { get; set; }
        public int LogLevel { get; set; }
    }
}