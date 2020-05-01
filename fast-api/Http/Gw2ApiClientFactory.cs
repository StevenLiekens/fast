using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fast_api.Http
{
    public class Gw2ApiClientFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public Gw2ApiClientFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Gw2ApiClient Create()
        {
            return _serviceProvider.GetRequiredService<Gw2ApiClient>();
        }
    }
}
