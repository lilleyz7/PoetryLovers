﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using PoetryLovers.DTO;
using PoetryLovers.IServices;
using PoetryLovers.Services;
using PoetryLovers.Utilities;
using System.Security.Claims;

namespace PoetryLovers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PoetryController : ControllerBase
    {
        public readonly IPoemRepo _repo;
        public readonly ILogger _logger;
        public PoetryController(IPoemRepo repo, ILogger logger)
        { 
            _repo = repo;
            _logger = logger;
        }

        [Authorize]
        [EnableRateLimiting("getLimiter")]
        [HttpGet("/byTitle/{title}")]
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
                    _logger.LogInformation($"Received Poem {poemResult.poem.Title}");
                    return Ok(poemResult.poem);
                }
                else
                {
                    _logger.LogError($"Failed request: {poemResult.error}");
                    return BadRequest(poemResult.error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed request: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [EnableRateLimiting("getLimiter")]
        [HttpGet("/random")]
        public async Task<IActionResult> GetRandomPoem()
        {
            try
            {
                var poemResult = await _repo.GetRandomPoem();
                if (poemResult.poem is not null)
                {
                    _logger.LogInformation($"Received Poem {poemResult.poem.Title}");
                    return Ok(poemResult.poem);
                }

                _logger.LogError($"Failed request: {poemResult.error}");
                return BadRequest(poemResult.error);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed request: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [EnableRateLimiting("getLimiter")]
        [HttpGet("/byAuthor/{author}/{count}")]
        public async Task<IActionResult> GetPoemsByAuthor(string author, int count)
        {
            if (string.IsNullOrEmpty(author)) { return BadRequest("Invalid author"); }

            try
            {
                var poems = await _repo.GetPoemsByAuthor(author, count);

                if (poems is not null) { return Ok(poems); }
                else { return NotFound($"No poems for author {author}"); }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed request: {ex.Message}");
                return BadRequest($"Could not find {author}: {ex.Message}");
            }

        }

        [Authorize]
        [EnableRateLimiting("postLimiter")]
        [HttpPost("/save")]
        public async Task<IActionResult> SavePoemToUser(PoemDTO poemToAdd)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            if (poemToAdd is null)
            {
                return BadRequest("Invalid body");
            }

            try
            {

                await _repo.SavePoem(poemToAdd, userId);
                return Ok("Poem Added");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed request: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [EnableRateLimiting("getLimiter")]
        [HttpGet("/mySaves/{count}")]
        public async Task<IActionResult> GetUserSaves(int count)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var poems = await _repo.GetSavedPoems(userId, count);
                if (poems is not null)
                {
                    return Ok(poems);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed request: {ex.Message}");
                return BadRequest(ex.Message);
            }

        }
    }
}
