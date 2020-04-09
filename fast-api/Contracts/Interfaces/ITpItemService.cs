using fast_api.Contracts.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fast_api.Contracts.Interfaces
{
    public interface ITpItemService
    {
        Task<List<Item>> GetItemPricesFromApi(int[] ids);
    }
}
