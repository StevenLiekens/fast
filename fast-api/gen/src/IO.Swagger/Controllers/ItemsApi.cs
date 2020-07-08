/*
 * Fast Gaming Community API
 *
 * API serving data gathered and analyzed by our lord and savior Cornix
 *
 * OpenAPI spec version: 0.0.1
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using IO.Swagger.Attributes;

using Microsoft.AspNetCore.Authorization;
using IO.Swagger.Models;

namespace IO.Swagger.Controllers
{ 
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class ItemsApiController : ControllerBase
    { 
        /// <summary>
        /// List all items for category with id
        /// </summary>
        /// <param name="id">Category Id</param>
        /// <response code="200">An array of category items</response>
        [HttpGet]
        [Route("/api/v1/categories/{id}/items")]
        [ValidateModelState]
        [SwaggerOperation("GetItemsForCategory")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Item>), description: "An array of category items")]
        public virtual IActionResult GetItemsForCategory([FromRoute][Required]string id)
        { 
            //TODO: Uncomment the next line to return response 200 or use other options such as return this.NotFound(), return this.BadRequest(..), ...
            // return StatusCode(200, default(List<Item>));
            string exampleJson = null;
            exampleJson = "[ {\n  \"buy\" : 6,\n  \"sell\" : 1,\n  \"name\" : \"name\",\n  \"id\" : \"id\"\n}, {\n  \"buy\" : 6,\n  \"sell\" : 1,\n  \"name\" : \"name\",\n  \"id\" : \"id\"\n} ]";
            
                        var example = exampleJson != null
                        ? JsonConvert.DeserializeObject<List<Item>>(exampleJson)
                        : default(List<Item>);            //TODO: Change the data returned
            return new ObjectResult(example);
        }
    }
}