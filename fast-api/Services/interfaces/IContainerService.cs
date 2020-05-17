using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.Contracts.DTO;

namespace fast_api.Services.interfaces
{
    public interface IContainerService
    {
        Task<List<ContainerDTO>> GetAsync();
        Task DeleteAsync(int id);
        Task AddOrUpdateAsync(ContainerDTO containerDto);
        Task UpdatePricesAsync();
    }
}