using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using Microsoft.AspNetCore.Cors;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.Contracts.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fast_api.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TpItemsController : ControllerBase
    {
        private readonly ITpItemService _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public TpItemsController(ITpItemService service, IMapper mapper, ILoggerFactory logger)
        {
            _mapper = mapper;
            _service = service;
            _logger = logger.CreateLogger(typeof(TpItemsController));
        }

        [EnableCors("AllowOrigin")]
        [HttpGet]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        public async Task<ActionResult> Get()
        {
            var items = await _service.GetItemPricesFromApi();
            //_logger.LogError("dis error");
            return Ok(_mapper.Map<List<Item>, IList<ItemDTO>>(items));
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        public async Task<ActionResult> Get(int[] ids)
        {
            var items = await _service.GetItemPricesFromApi(ids);
            //_logger.LogError("dis error");
            return Ok(_mapper.Map<List<Item>, IList<ItemDTO>>(items));
        }
    }
}
