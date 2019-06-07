using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using CarWashBooking.Extensions;

namespace CarWashBooking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging ((  hostingcontext,logging)=>
            {
                logging.AddLoggingConfiguration(hostingcontext.Configuration);
            })
            .UseStartup<Startup>();
    }
}
