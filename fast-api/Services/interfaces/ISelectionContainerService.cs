using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.EntityFramework.Entities;

namespace fast_api.Services.interfaces
{
    public interface ISelectionContainerService
    {
        Task<List<SelectionContainer>> GetAsync();
        Task DeleteAsync(int id);
        Task AddOrUpdateAsync(SelectionContainer selectionContainer);
        Task UpdatePriceAsync(SelectionContainer selectionContainer);
    }
}