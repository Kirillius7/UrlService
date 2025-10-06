using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UrlService.Models;


[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager; // частина бібліотеки Identity для авторизації
    private readonly UserManager<AppUser> _userManager; // частина бібліотеки Identity для керування користувачами
    private readonly IConfiguration _config;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        // пошук користувача за ім'ям
        var user = await _userManager.FindByNameAsync(model.UserName);
        if (user == null) return Unauthorized();

        // перевірка валідності введеного пароля
        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded) return Unauthorized();

        // Формування claims (список інформації про користувача для збереження в токені)
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email ?? "")
        };

        // надання користувачу відповідної ролі
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        // Генерація токена, використовуючи секретний ключ та підпис
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"], // видавець
            audience: _config["Jwt:Audience"], // отримувач
            claims: claims, // дані про користувача
            expires: DateTime.Now.AddHours(1), // термін придатності
            signingCredentials: creds // підпис для захисту
        );

        // повернення JSON із токеном, іменем та правами користувача 
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), user.UserName, Roles = roles });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new AppUser { UserName = model.UserName, Email = model.Email };
        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "User"); // звичайний користувач за замовчуванням
        return Ok();
    }
}

public class LoginModel
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class RegisterModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
