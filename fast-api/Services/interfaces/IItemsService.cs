using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.EntityFramework.Entities;

namespace fast_api.Services.interfaces
{
    public interface IItemsService
    {
        Task<List<Item>> GetAsync();
        Task FetchItemsFromApiAsync();
        Task UpdatePricesAsync();
    }
}