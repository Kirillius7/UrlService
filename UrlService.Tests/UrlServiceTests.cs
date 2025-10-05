using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;

using UrlService.Service;
using UrlService.Models;
using UrlService.Controllers;

namespace UrlService.Tests
{
    public class UrlServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString()) // унікальне ім’я для кожного тесту
               .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task CreateNewUrlAsync()
        {
            var context = GetDbContext();
            var service = new UrlServiceClass(context);

            var dto = new CreateUrlDto { OriginalUrl = "https://example.com" };
            var result = await service.CreateUrlAsync(dto, "Tester");

            Assert.NotNull(result);
            Assert.Equal("https://example.com", result.OriginalUrl);
            Assert.Equal("Tester", result.CreatedBy);
            Assert.False(string.IsNullOrEmpty(result.ShortCode));
        }

        [Fact]
        public async Task CreateDuplicateUrlAsync()
        {
            var context = GetDbContext();
            var service = new UrlServiceClass(context);

            var dto = new CreateUrlDto { OriginalUrl = "https://duplicate.com" };
            await service.CreateUrlAsync(dto, "User1");

            await Assert.ThrowsAsync<Exception>(() => service.CreateUrlAsync(dto, "User2"));
        }

        [Fact]
        public async Task GetAllUrlsAsync()
        {
            var context = GetDbContext();
            var service = new UrlServiceClass(context);

            await service.CreateUrlAsync(new CreateUrlDto { OriginalUrl = "https://a.com" }, "User");
            await service.CreateUrlAsync(new CreateUrlDto { OriginalUrl = "https://b.com" }, "User");

            var allUrls = await service.GetAllUrlsAsync();

            Assert.Equal(2, allUrls.Count);
            Assert.Contains(allUrls, u => u.OriginalUrl == "https://a.com");
            Assert.Contains(allUrls, u => u.OriginalUrl == "https://b.com");
        }
    }
}
