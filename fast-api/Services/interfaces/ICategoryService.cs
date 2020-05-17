using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.EntityFramework.Entities;

namespace fast_api.Services.interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAsync();
        Task DeleteAsync(int id);
        Task AddOrUpdateAsync(Category category);
        Task UpdatePricesAsync();
    }
}