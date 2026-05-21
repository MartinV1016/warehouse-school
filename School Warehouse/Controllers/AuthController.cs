using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WarehouseAPI.DTOs	;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
	private static List<User> users = new()
	{
		new User { Id = 1, Username = "admin", Password = "admin123", Role = "admin" },
		new User { Id = 2, Username = "stationery", Password = "pass123", Role = "stationery" }
	};

	private string key = "supersecretkey_that_is_long_enough_to_be_secure_123!";

	[HttpPost("login")]
	public IActionResult Login(Login request)
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
		});
	}
}