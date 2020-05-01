using AutoMapper;
using fast_api.Config;
using fast_api.Config.AutoMapper;
using fast_api.Contracts.Interfaces;
using fast_api.DataAccess.Repositories;
using fast_api.Http;
using fast_api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Data;
using StackExchange.Redis;
using System;
using System.Data;
using System.Diagnostics;

namespace fast_api
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var apiSettingsSection = _configuration.GetSection("Gw2ApiEndpoints");
            var redisSettingsSection = _configuration.GetSection("Redis");
            var loggingSettingsSection = _configuration.GetSection("Logging");
            var allowedCorsDomainsSection = _configuration.GetSection("CORSDomains");
            services.Configure<Gw2ApiEndpoints>(apiSettingsSection);
            services.Configure<RedisConfig>(redisSettingsSection);
            services.Configure<CORSDomains>(allowedCorsDomainsSection);

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 433;
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin", builder =>
                {
                    //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowAnyHeader().AllowAnyMethod();
                    //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "0.0.0.0").AllowAnyHeader().AllowAnyMethod();
                    //builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "rofl.test").AllowAnyHeader().AllowAnyMethod();
                    //builder.WithOrigins("https://fast.farming-community.eu", "http://localhost:8080", "http://rofl.test:8080").AllowAnyHeader().AllowAnyMethod();
                    //builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    builder.WithOrigins(allowedCorsDomainsSection.Get<CORSDomains>().AllowedCorsDomains.Split(';')).AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddHttpClient<Gw2ApiClient>(x => { x.BaseAddress = new Uri(apiSettingsSection.Get<Gw2ApiEndpoints>().Gw2ApiRoot); });
            services.AddSingleton<Gw2ApiClientFactory>();

            services.AddTransient<ITpItemService, TpItemService>();
            services.AddTransient<IGw2ApiRepository, Gw2ApiRepository>();
            services.AddSingleton<ICacheRepository, Gw2ItemRedisRepository>();
            services.AddSingleton<IConnectionMultiplexer>(x =>
            {
                try
                {
                    return ConnectionMultiplexer.Connect(redisSettingsSection.Get<RedisConfig>().RedisServerIp);
                }
                catch (RedisConnectionException ex)
                {
                    Log.Error("Redis error. Is the redis instance running and properly configured?");
                    return null;
                }
            });

            services.AddControllers();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "FAST Gw2 API", Version = "v1" });
            });

            services.AddAutoMapper(typeof(ItemMapperProfile));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "FAST Gw2 API v1");
                x.RoutePrefix = string.Empty;
            });

            //app.UseExceptionHandler(errorApp =>
            //{
            //    errorApp.Run(async context =>
            //    {
            //        context.Response.StatusCode = 500;
            //        context.Response.ContentType = "application/json";


            //        var exceptionHandlerPathFeature =
            //            context.Features.Get<IExceptionHandlerPathFeature>();

            //        // Use exceptionHandlerPathFeature to process the exception (for example, 
            //        // logging), but do NOT expose sensitive error information directly to 
            //        // the client.
            //        var errorType = exceptionHandlerPathFeature?.Error;
            //        switch (errorType)
            //        {
            //            case RedisConnectionException err:
            //                await context.Response.WriteAsync(JsonConvert.SerializeObject(new JsonResult(new { Error = "Redis error!" }).Value));
            //                break;
            //            default:
            //                await context.Response.WriteAsync(JsonConvert.SerializeObject(new JsonResult(new { Error = "Redis error!" }).Value));
            //                break;
            //        };
            //    });
            //});

            app.UseRouting();

            app.UseCors("AllowOrigin");

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
                endpoints.MapControllers();
            });
        }
    }
}
