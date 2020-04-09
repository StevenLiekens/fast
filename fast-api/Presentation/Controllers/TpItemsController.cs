using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace fast_api.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TpItemsController : ControllerBase
    {
        private readonly ITpItemService _service;
        public TpItemsController(ITpItemService service)
        {
            _service = service;
        }

        //public async Task<List<ItemPrice>> Get()
        //{
        //    return await _service.GetItemPricesFromApi();
        //}

        [HttpPost]
        public async Task<List<Item>> Get(int[] ids)
        {
            //var idArray = ids.Split(',');
            return await _service.GetItemPricesFromApi(ids);
        }
    }
}
