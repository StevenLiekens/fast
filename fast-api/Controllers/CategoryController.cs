using System;
using System.Threading.Tasks;
using fast_api.Contracts.DTO;
using fast_api.Services.interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace fast_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<SelectionContainerController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<SelectionContainerController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CategoryDTO), 200)]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _categoryService.GetAsync());
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post(CategoryDTO category)
        {
            try
            {
                await _categoryService.AddOrUpdateAsync(category);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
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