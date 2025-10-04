using UrlService.Models;

namespace UrlService.Service
{
    public interface IUrlService
    {
        Task<List<ShortUrl>> GetAllUrlsAsync();
        Task<ShortUrl> GetByIdAsync(int id);
        Task<ShortUrl> CreateUrlAsync(ShortUrl request);
        Task<bool> DeleteUrlAsync(int id, string currentUser, bool isAdmin);
    }
}
