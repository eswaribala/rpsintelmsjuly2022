using CatalogAPIDockerCompose.Auth;
using CatalogAPIDockerCompose.Models;
using CatalogAPIDockerCompose.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPIDockerCompose.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiVersion("1.0")]
   // [ApiVersion("1.1")]
   // [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private ICatalogRepo CatalogRepo;

        public CatalogController(ICatalogRepo catalogRepo)
        {
            CatalogRepo = catalogRepo;

        }

        // GET: api/<VehicleController>
        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<IEnumerable<Catalog>> Get()
        {
            return await this.CatalogRepo.GetCatalogs();
        }



        // GET api/<VehicleController>/5
        [HttpGet("{CatalogId}")]
        public async Task<Catalog> Get(long CatalogId)
        {
            return await this.CatalogRepo.GetCatalogById(CatalogId);
        }

        // POST api/<VehicleController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Catalog  Catalog)
        {
            await this.CatalogRepo.AddCatalog(Catalog);
            return CreatedAtAction(nameof(Get),
                            new { id =Catalog.CatalogId }, Catalog);

        }

        // PUT api/<VehicleController>/5
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] Catalog Catalog)
        {
            await this.CatalogRepo.UpdateCatalog(Catalog);
            return CreatedAtAction(nameof(Get),
                            new { id = Catalog.CatalogId }, Catalog);
        }

        // DELETE api/<VehicleController>/5
        [HttpDelete("{CatalogId}")]
        public async Task<IActionResult> Delete(long CatalogId)
        {

            if (await this.CatalogRepo.DeleteCatalog(CatalogId)) 
                return new OkResult();
            else
                return new BadRequestResult();

        }

    }
}
