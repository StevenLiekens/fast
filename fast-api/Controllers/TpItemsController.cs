using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.Contracts.Interfaces;
using fast_api.Contracts.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fast_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            try
            {
                var items = await _service.GetItemPricesFromApi();
                return Ok(_mapper.Map<List<Item>, IList<ItemDTO>>(items));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem("An error occured");
            }
        }

        [EnableCors("AllowOrigin")]
        [HttpPost]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        public async Task<ActionResult> Get(int[] ids)
        {
            try
            {
                var items = await _service.GetItemPricesFromApi(ids);
                return Ok(_mapper.Map<List<Item>, IList<ItemDTO>>(items));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem("An error occured");
            }
        }
    }
}