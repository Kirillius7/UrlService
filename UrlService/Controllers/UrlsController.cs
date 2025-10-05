using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<IActionResult> Create([FromBody] CreateUrlDto dto)
        {
            try
            {
                var currentUser = User.Identity.Name;
                var url = await _urlService.CreateUrlAsync(dto, currentUser);
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
            var currentUser = User.Identity.Name;
            var isAdmin = User.IsInRole("Admin");

            var result = await _urlService.DeleteUrlAsync(id, currentUser, isAdmin);
            if (!result) return Forbid();
            return NoContent();
        }

        [HttpGet("go/{shortCode}")]
        public async Task<IActionResult> GoToOriginal(string shortCode, [FromQuery] bool info = false)
        {
            var url = await _urlService.GoToOriginalAsync(shortCode, info);

            // якщо додано info=true, показуємо деталі
            if (info)
            {
                return Ok(new
                {
                    url.OriginalUrl,
                    url.ShortCode,
                    url.CreatedBy,
                    url.CreatedDate
                });
            }

            // редірект на оригінальний сайт
            return Redirect(url.OriginalUrl);
        }
    }
}
