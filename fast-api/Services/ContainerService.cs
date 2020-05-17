using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.EntityFramework;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace fast_api.Services
{
    class ContainerService : IContainerService
    {
        private readonly FastContext _context;
        private readonly IMapper _mapper;

        public ContainerService(FastContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<ContainerDTO>> GetAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteAsync(int id)
        {
            await DeleteByIdAsync(id);
        }

        public async Task AddOrUpdateAsync(ContainerDTO containerDto)
        {
            await DeleteByIdAsync(containerDto.ContainerId);
        }

        public Task UpdatePricesAsync()
        {
            throw new System.NotImplementedException();
        }

        private async Task DeleteByIdAsync(int id)
        {
            var entry = await _context.Containers.FindAsync(id);
            if (entry != null)
            {
                _context.Remove(entry);
                await _context.SaveChangesAsync();
            }
        }
    }
}