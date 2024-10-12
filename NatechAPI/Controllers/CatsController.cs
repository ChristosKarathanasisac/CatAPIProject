using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NatechAPI.Data;
using NatechAPI.Services;

namespace NatechAPI.Controllers
{
    // localhost:xxxx/api/cats
    [Route("api/[controller]")]
    [ApiController]
    public class CatsController : ControllerBase
    {
        private readonly CatsService catsService;

        public CatsController(CatsService catsService)
        {
            this.catsService = catsService;
        }

        [HttpGet]
        [Route("{page:int}")]
        public IActionResult Get(int page) 
        {
            if (catsService.AddCatsToDb())
            {
                return Ok();
            }
            else 
            {
                return BadRequest();
            }
            
        }
    }
}
