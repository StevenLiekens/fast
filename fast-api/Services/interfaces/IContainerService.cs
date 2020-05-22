using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.EntityFramework.Entities;

namespace fast_api.Services.interfaces
{
    public interface IContainerService
    {
        Task<List<Container>> GetAsync();
        Task DeleteAsync(int id);
        Task AddOrUpdateAsync(Container container);
        Task UpdatePriceAsync(Container container);
    }
}