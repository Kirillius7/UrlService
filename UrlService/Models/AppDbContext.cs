using Microsoft.EntityFrameworkCore;
using System;

namespace UrlService.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<ShortUrl> shortUrl { get; set; } // оголошення таблиці (колекції сутностей)
        // для роботи із даними як із колекцією (а не таблицею і рядком SQL)

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        // підключення до відповідної БД, врахування додаткового функціоналу (lazy loading, логування)
        // як поводитись із транзакціями та міграціями
    }
}
