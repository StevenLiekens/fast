using fast_api.Contracts.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace fast_api.Contracts.Interfaces
{
    public interface IGw2ApiRepository
    {
        Task<List<int>> FetchAllItemIdsFromApi(CancellationToken cancellationToken);
        Task<List<Item>> FetchAllItemsFromApi(CancellationToken cancellationToken, int[] filter = null);
        Task<List<ItemPrice>> FetchAllItemPricesFromApi(CancellationToken cancellationToken, int[] filter = null);
        Task<List<Item>> FetchItemPricesFromApi(CancellationToken cancellationToken, int[] itemIds);
        //Task<Item> FetchItemPriceFromApi(int id);
    }
}
