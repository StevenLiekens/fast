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
    public class SelectionContainerController : ControllerBase
    {
        private readonly ISelectionContainerService _selectionContainerService;
        private readonly IMapper _mapper;
        private readonly ILogger<SelectionContainerController> _logger;

        public SelectionContainerController(ISelectionContainerService selectionContainerService, IMapper mapper, ILogger<SelectionContainerController> logger)
        {
            _selectionContainerService = selectionContainerService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SelectionContainerDTO), 200)]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _selectionContainerService.GetAsync());
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(SelectionContainerDTO selectionContainer)
        {
            try
            {
                await _selectionContainerService.AddAsync(selectionContainer);
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