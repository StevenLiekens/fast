using fast_api.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace fast_api.Contracts.Interfaces
{
    public interface IGw2ApiClient
    {
        Task<List<int>> FetchAllItemIdsFromApi(CancellationToken cancellationToken, string endpoint);
        Task<List<Item>> FetchAllItemsFromApi(CancellationToken cancellationToken, string endpoint);
        Task<List<ItemPrice>> FetchAllItemPricesFromApi(CancellationToken cancellationToken, string endpoint);
    }
}
