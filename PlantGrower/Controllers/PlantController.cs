using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlantGrower.Models;
using PlantGrower.Services;

namespace PlantGrower.Controllers
{
    [Route("api/[controller]")]
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
            return await _plantService.Get().ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ActionResult<StatusCodeResult>> Create(Plant plant)
        {
            await _plantService.Create(plant).ConfigureAwait(false);
            return NoContent();
        }
    }
}