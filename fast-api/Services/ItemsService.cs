using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.Contracts.Interfaces;
using fast_api.EntityFramework;
using fast_api.Services.interfaces;
using Microsoft.EntityFrameworkCore;

namespace fast_api.Services
{
    public class ItemsService : IItemsService
    {
        private readonly FastContext _context;
        private readonly IMapper _mapper;
        private readonly ITpItemService _tpItemService;

        public ItemsService(FastContext context, IMapper mapper, ITpItemService tpItemService)
        {
            _context = context;
            _mapper = mapper;
            _tpItemService = tpItemService;
        }

        public async Task<List<ItemDTO>> GetAsync()
        {
            return _mapper.Map<List<ItemDTO>>(await _context.Items.Take(50).ToListAsync());
        }

        public async Task FetchItemsFromApiAsync()
        {
            var items = await _tpItemService.GetItemPricesFromApi();
            await _context.AddRangeAsync(_mapper.Map<List<Item>>(items));
            await _context.SaveChangesAsync();
        }
    }
}