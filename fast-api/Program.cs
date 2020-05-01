using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using fast_api.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace fast_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("hostsettings.json", optional: true)
                .AddCommandLine(args)
                .Build();

            var logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.Logger(x =>
                    {
                        if (config.GetSection("Logging").GetValue<bool>("LogToFile") == true)
                        {
                            x.WriteTo.File("information.log", rollOnFileSizeLimit: true, fileSizeLimitBytes: 20_971_520);
                            x.Filter.ByExcluding(x => x.Level == Serilog.Events.LogEventLevel.Error);
                            x.Filter.ByExcluding(x => x.Level == Serilog.Events.LogEventLevel.Fatal);
                            x.Filter.ByExcluding(x => x.Level == Serilog.Events.LogEventLevel.Warning);
                        }
                    })
                    .WriteTo.Logger(x =>
                    {
                        x.WriteTo.File("error.log");
                        x.MinimumLevel.Warning();
                    })
                    .CreateLogger();

            return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseStartup<Startup>();
                       webBuilder.UseUrls("http://*:50699");
                       webBuilder.UseConfiguration(config);
                       webBuilder.UseSerilog(logger);
                   });
        }
    }
}
