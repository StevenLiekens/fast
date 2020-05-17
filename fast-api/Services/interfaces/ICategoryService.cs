using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.Contracts.DTO;

namespace fast_api.Services.interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDTO>> GetAsync();
        Task DeleteAsync(int id);
        Task AddOrUpdateAsync(CategoryDTO categoryDto);
        Task UpdatePricesAsync();
    }
}