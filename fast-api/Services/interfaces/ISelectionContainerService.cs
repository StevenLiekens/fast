using System.Collections.Generic;
using System.Threading.Tasks;
using fast_api.Contracts.DTO;

namespace fast_api.Services.interfaces
{
    public interface ISelectionContainerService
    {
        Task<List<SelectionContainerDTO>> GetAsync();
        Task AddAsync(SelectionContainerDTO selectionContainer);
    }
}