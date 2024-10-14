using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NatechAPI.Data;
using NatechAPI.Models.ViewModels;
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
                    AddCatsResponseVM response = new AddCatsResponseVM();
                    response.status = "ok";
                    response.addedCatsNum = insertedCats.Count;
                    response.addedCats = insertedCats;
                    return Ok(response);
                }
                else
                {
                    return StatusCode(500, $"Internal server error.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("You must pass an Id");
                }

                ReturnedCatsVM resp = await catsService.GetCatById(id);
                if (resp != null)
                {
                    if (string.IsNullOrEmpty(resp.CatId)) 
                    {
                        return NotFound();
                    }
                    return Ok(resp);
                }
                else
                {
                    return StatusCode(500, $"Internal server error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpGet]
        [Route("{page:int}/{pageSize:int}")]
        public async Task<IActionResult> Get(int page,int pageSize)
        {
            try
            {
                if (page <= 0)
                {
                    return BadRequest("Page number must be greater than 0.");
                }

                if (pageSize <= 0)
                {
                    return BadRequest("Page size must be greater than 0.");
                }

                GetCatsPegResponseVM resp = await catsService.GetCatsWithPegination(page, pageSize);
                if (resp != null)
                {
                    return Ok(resp);
                }
                else
                {
                    return StatusCode(500, $"Internal server error");
                }
            }
            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server error");
            }
        }

        [HttpGet]
        [Route("{tag}/{page:int}/{pageSize:int}")]
        public async Task<IActionResult> Get(string tag,int page, int pageSize)
        {
            try
            {
                if (page <= 0)
                {
                    return BadRequest("Page number must be greater than 0.");
                }

                if (pageSize <= 0)
                {
                    return BadRequest("Page size must be greater than 0.");
                }
                if (string.IsNullOrEmpty(tag))
                {
                    return BadRequest("Tag was empty.");
                }

                GetCatsPegResponseVM resp = await catsService.GetCatsWithPegination(tag.Trim(),page, pageSize);
                if (resp != null)
                {
                    return Ok(resp);
                }
                else
                {
                    return StatusCode(500, $"Internal server error");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error");
            }
        }
    }
}
