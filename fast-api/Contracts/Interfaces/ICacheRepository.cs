using fast_api.Contracts.Models;
using System.Threading.Tasks;

namespace fast_api.Contracts.Interfaces
{
    public interface ICacheRepository
    {
        Task<bool> CheckIfCached(int id);
        Task WriteToCache(Item item);
        Task<Item> ReadFromCache(int id);
    }
}
