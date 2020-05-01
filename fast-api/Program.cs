using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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
                        if (config.GetSection("Logging").GetValue<bool>("LogToFile"))
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
