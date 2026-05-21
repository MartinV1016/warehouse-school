using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarehouseAPI.Data;
using WarehouseAPI.DTOs;
using WarehouseAPI.Models;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
	/*private static List<User> users = new()
	{
		new User { Id = 1, Username = "admin", Password = "admin123", Role = "admin" },
		new User { Id = 2, Username = "stationery", Password = "pass123", Role = "stationery" }
	};*/

	private readonly WarehouseDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(WarehouseDbContext context, IConfiguration configuration)
	{ _context = context; }	

	[HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Login request)
    {

        var jwtKey = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt key is missing from configuration");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid username or password"));
        }

   
        var passwordHasher = new PasswordHasher<User>();
        var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return Unauthorized(ApiResponse<object>.Fail("Invalid username or password"));
        }

        var claims = new[]
        {
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Name, user.Username) // Great for your "WhoAmI" endpoint!
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                SecurityAlgorithms.HmacSha256)
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        var loginResult = new LoginResponseDto(tokenString, user.Username, user.Role);
        var response = ApiResponse<LoginResponseDto>.Success(loginResult, $"{user.Username}, you are logged in as {user.Role}");

        return Ok(response);


        /*public IActionResult Login(Login request)
        {
            var user = users.FirstOrDefault(u =>
                u.Username == request.Username &&
                u.Password == request.Password);

            if (user == null)
                return Unauthorized(ApiResponse<object>.Fail("Invalid username or password"));

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var loginResult= new LoginResponseDto(tokenString,user.Username,user.Role);

            var response= ApiResponse<LoginResponseDto>.Success(loginResult,$"{user.Username}, you are logged in as {user.Role}");

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });*/
    }
}