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

        [HttpPost]
        [Route("fetch")]
        public async Task<IActionResult> FetchCats()
        {
            try
            {
                HashSet<string> insertedCats = await catsService.AddCatsToDb();
                if (insertedCats != null)
                {
                    //Some message
                    return Ok("Cats data fetched and added to the database.");
                }
                else
                {
                    return StatusCode(500, $"Internal server error.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpGet]
        //[Route("{page:int}")]
        //public async Task<IActionResult> Get(int page) 
        //{
            
            
        //}
    }
}
