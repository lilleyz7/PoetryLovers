using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PoetryLovers.IServices;
using PoetryLovers.Services;

namespace PoetryLovers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoetryController : ControllerBase
    {
        public readonly IPoemRepo _repo;
        public PoetryController(IPoemRepo repo)
        { 
            _repo = repo;
        }

        [HttpGet("/poems/{title}")]
        public async Task<IActionResult> GetPoemByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest("title");
            }
            try
            {
                var poemResult = await _repo.GetPoemByTitle(title);
                if (poemResult.poem is not null)
                {
                    return Ok(poemResult.poem);
                }
                else
                {
                    return BadRequest(poemResult.error);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/random")]
        public async Task<IActionResult> GetRandomPoem()
        {
            try
            {
                var poemResult = await _repo.GetRandomPoem();
                if (poemResult.poem is not null)
                {
                    return Ok(poemResult.poem);
                }
                return BadRequest(poemResult.error);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
