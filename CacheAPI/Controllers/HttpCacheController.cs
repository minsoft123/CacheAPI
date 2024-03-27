using CacheAPI.Models;
using CacheAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CacheAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HttpCacheController : ControllerBase
    {
        private readonly ICacheService _cacheService;

        public HttpCacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet("Get/{key}")]
        [Description("Get cache entry by key.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([Required] string key)
        {
            try
            {
                return Ok(await _cacheService.GetAsync(key));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Upsert")]
        [Description("Add or Update cache.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status507InsufficientStorage)]
        public async Task<IActionResult> Upsert([FromBody] CacheEntryModel entry)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _cacheService.UpsertAsync(entry);
            }
            catch (OutOfCacheSizeLimitException ex)
            {
                return Problem(statusCode: StatusCodes.Status507InsufficientStorage, detail: ex.Message);
            }

            return Ok();
        }

        [HttpPost("UpsertWithExpirationOptions")]
        [Description("Add or Update cache.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Upsert([FromBody] CacheEntryWithExpirationOptionsModel entry)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await Task.Run(() => _cacheService.UpsertAsync(entry));
            return Ok();
        }

        [HttpDelete("Remove/{key}")]
        [Description("Remove cache entry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Remove([Required] string key)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _cacheService.RemoveAsync(key);
            return NoContent();
        }

        [HttpDelete("RemoveAll")]
        [Description("Remove all cache entries.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveAll()
        {
            await _cacheService.RemoveAllAsync();
            return NoContent();
        }

        [HttpGet("GetCacheStatistics")]
        [Description("Get current statistics of cache.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCacheStatistics()
        {
            return Ok(await _cacheService.GetCacheStatisticsAsync());
        }
    }
}
