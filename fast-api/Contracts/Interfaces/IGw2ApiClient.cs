using fast_api.Contracts.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace fast_api.Contracts.Interfaces
{
    public interface IGw2ApiClient
    {
        Task<List<int>> FetchAllItemIdsAsync(CancellationToken cancellationToken);
        Task<List<Item>> FetchItemsForIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids);
        Task<List<ItemPrice>> FetchItemPricesForIdsAsync(CancellationToken cancellationToken, IEnumerable<int> ids);
    }
}
