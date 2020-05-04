using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.Contracts.DTO;

namespace fast_api.Services.interfaces
{
    public interface IItemsService
    {
        Task<List<ItemDTO>> GetAsync();
        Task FetchItemsFromApiAsync();
        Task UpdatePrices();
    }
}