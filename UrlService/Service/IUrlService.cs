using Microsoft.AspNetCore.Mvc;
using UrlService.Models;

namespace UrlService.Service
{
    public interface IUrlService
    {
        Task<List<ShortUrl>> GetAllUrlsAsync();
        Task<ShortUrl> GetByIdAsync(int id);
        Task<ShortUrl> CreateUrlAsync(CreateUrlDto request, string currentUser);
        Task<bool> DeleteUrlAsync(int id, string currentUser, bool isAdmin);
        Task<ShortUrl> GoToOriginalAsync(string shortCode, [FromQuery] bool info = false);
    }
}
