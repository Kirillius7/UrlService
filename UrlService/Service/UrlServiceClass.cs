using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlService.Models;

namespace UrlService.Service
{
    public class UrlServiceClass : IUrlService
    {
        private readonly AppDbContext _context;

        public UrlServiceClass(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShortUrl>> GetAllUrlsAsync()
        {
            return await _context.shortUrl.ToListAsync();
        }

        public async Task<ShortUrl> GetByIdAsync(int id)
        {
            return await _context.shortUrl.FindAsync(id);
        }

        public async Task<ShortUrl> CreateUrlAsync(CreateUrlDto request, string currentUser)
        {
            // 1️ Перевірка на порожнє значення
            if (string.IsNullOrWhiteSpace(request.OriginalUrl))
                throw new Exception("URL cannot be empty.");

            // 2️ Перевірка на коректний формат
            if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out var uriResult)
                || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                throw new Exception("Invalid URL format. Please provide a valid link (http or https).");
            }

            if (_context.shortUrl.Any(u => u.OriginalUrl == request.OriginalUrl))
                throw new Exception("This URL already exists.");

            var shortUrl = new ShortUrl
            {
                OriginalUrl = request.OriginalUrl,
                ShortCode = Guid.NewGuid().ToString("N").Substring(0, 6),
                CreatedBy = currentUser ?? "Anonymous",
                CreatedDate = DateTime.UtcNow
            };

            _context.shortUrl.Add(shortUrl);
            await _context.SaveChangesAsync();

            return shortUrl;
        }


        public async Task<bool> DeleteUrlAsync(int id, string currentUser, bool isAdmin)
        {
            var url = await _context.shortUrl.FindAsync(id);
            if (url == null) return false;

            // перевірка прав
            if (!isAdmin && url.CreatedBy != currentUser)
                return false;

            _context.shortUrl.Remove(url);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ShortUrl> GoToOriginalAsync(string shortCode, [FromQuery] bool info = false)
        {
            var url = await _context.shortUrl.FirstOrDefaultAsync(u => u.ShortCode == shortCode);
            return url;
        }
    }
}
