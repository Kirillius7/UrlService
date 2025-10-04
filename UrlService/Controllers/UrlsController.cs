using Microsoft.AspNetCore.Mvc;
using UrlService.Models;
using UrlService.Service;

namespace UrlService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlsController : ControllerBase
    {
        private readonly IUrlService _urlService;

        public UrlsController(IUrlService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var urls = await _urlService.GetAllUrlsAsync();
            return Ok(urls);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var url = await _urlService.GetByIdAsync(id);
            if (url == null) return NotFound();
            return Ok(url);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ShortUrl request)
        {
            try
            {
                var url = await _urlService.CreateUrlAsync(request);
                return Ok(url);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Тут поки що передаємо "user1" і false як приклад
            var result = await _urlService.DeleteUrlAsync(id, "user1", false);
            if (!result) return Forbid();
            return NoContent();
        }
    }
}
