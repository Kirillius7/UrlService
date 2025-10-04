using UrlService.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ShortUrl> CreateUrlAsync(ShortUrl request)
        {
            if (_context.shortUrl.Any(u => u.OriginalUrl == request.OriginalUrl))
                throw new Exception("This URL already exists.");

            request.CreatedDate = DateTime.UtcNow;
            _context.shortUrl.Add(request);
            await _context.SaveChangesAsync();

            // Генеруємо короткий код
            request.ShortCode = ConvertToShortCode(request.Id);
            await _context.SaveChangesAsync();

            return request;
        }

        public async Task<bool> DeleteUrlAsync(int id, string currentUser, bool isAdmin)
        {
            var url = await _context.shortUrl.FindAsync(id);
            if (url == null) return false;

            // Перевірка прав
            if (!isAdmin && url.CreatedBy != currentUser)
                return false;

            _context.shortUrl.Remove(url);
            await _context.SaveChangesAsync();
            return true;
        }

        private string ConvertToShortCode(int id)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = "";
            var num = id;
            while (num > 0)
            {
                result = chars[num % 62] + result;
                num /= 62;
            }
            return result;
        }
    }
}
