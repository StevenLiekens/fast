using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fast_api.Config
{
    public class Gw2ApiEndpoints
    {
        public string Gw2ApiCommercePricesEndpoint { get; set; }
        public string Gw2ApiItemEndpoint { get; set; }
    }
}
