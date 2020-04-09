using fast_api.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fast_api.Contracts.Interfaces
{
    public interface IApiFetchClient
    {
        //Task<List<ItemPrice>> FetchItemsFromApi();
        Task<Item> FetchItemPriceFromApi(int id);
        Task<List<Item>> FetchItemPricesFromApi(int[] itemIds);
    }
}
