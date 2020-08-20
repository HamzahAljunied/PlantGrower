using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;
using MongoDB.Driver;
using PlantGrower.Models;
using PlantGrower.Services;

namespace PlantGrower.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly PlantService _plantService;

        public PlantsController(PlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet]        
        public async Task<ActionResult<List<Plant>>> Get()
        {
            var queries = HttpContext.Request.Query;            
            if(queries.Count == 0)
            {
                return await _plantService.Get().ConfigureAwait(false);
            }

            return await _plantService.Get(queries).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Plant plant)
        {
            await _plantService.Create(plant).ConfigureAwait(false);
            var statusCode = new StatusCodeResult(200);            
            return statusCode;
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var queries = HttpContext.Request.Query;
            var res = await _plantService.Delete(queries).ConfigureAwait(false);
            return Ok(res);
        }
    }
}