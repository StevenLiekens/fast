using System;
using System.Threading.Tasks;
using AutoMapper;
using fast_api.Contracts.DTO;
using fast_api.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fast_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly IMapper _mapper;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(ICurrencyService currencyService, IMapper mapper, ILogger<CurrencyController> logger)
        {
            _mapper = mapper;
            _currencyService = currencyService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CurrencyTradeDTO), 200)]
        public async Task<ActionResult> GetAsync()
        {
            try
            {
                return Ok(await _currencyService.GetAsync());
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }

        [HttpGet]
        [Route("{currency}")]
        [ProducesResponseType(typeof(PriceDataDTO), 200)]
        public async Task<ActionResult> GetAsync([FromRoute]string currency)
        {
            try
            {
                return Ok(await _currencyService.GetCurrencyValueAsync(currency));
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddOrUpdateAsync(CurrencyTradeDTO currencyTrade)
        {
            try
            {
                await _currencyService.AddOrUpdateAsync(currencyTrade);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }
    }
}