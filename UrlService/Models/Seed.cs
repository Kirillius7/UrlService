using Microsoft.AspNetCore.Identity;

namespace UrlService.Models
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            // ідентифікація моделей на основі бібліотеки Identity
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            string[] roles = { "Admin", "User" };

            // перевірка наявності і створення ролей у базі
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // перевірка і створення початкового користувача у якості адміна
            var adminEmail = "admin@example.com";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new AppUser { UserName = "admin", Email = adminEmail };
                await userManager.CreateAsync(admin, "Admin123!"); // пароль
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
