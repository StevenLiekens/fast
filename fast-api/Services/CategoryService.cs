using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.EntityFramework;
using fast_api.Services.interfaces;
using Microsoft.Extensions.Logging;

namespace fast_api.Services
{
    class CategoryService : ICategoryService
    {
        private readonly FastContext _context;
        private readonly IMapper _mapper;

        public CategoryService(FastContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<CategoryDTO>> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteByIdAsync(id);
        }

        public async Task AddOrUpdateAsync(CategoryDTO categoryDto)
        {
            await DeleteByIdAsync(categoryDto.CategoryId);
        }

        public Task UpdatePricesAsync()
        {
            throw new System.NotImplementedException();
        }

        private async Task DeleteByIdAsync(int id)
        {
            var entry = await _context.Categories.FindAsync(id);
            if (entry != null)
            {
                _context.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}