﻿using System;
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
    public class ItemsController : ControllerBase
    {
        private readonly IItemsService _itemsService;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemsService itemsService, ILogger<ItemsController> logger)
        {
            _itemsService = itemsService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ItemDTO), 200)]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await _itemsService.GetAsync());
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }

        [HttpGet]
        [Route("fetch")]
        public async Task<ActionResult> FetchFromApi()
        {
            try
            {
                await _itemsService.FetchItemsFromApiAsync();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Error :(");
                return Problem($"An error occured: {e}");
            }
        }

        [HttpGet]
        [Route("UpdatePricesAsync")]
        public async Task<ActionResult> UpdatePrices()
        {
            try
            {
                await _itemsService.UpdatePricesAsync();
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
