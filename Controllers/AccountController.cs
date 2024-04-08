using chat_service.MyDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace chat_service.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountController : ControllerBase
	{
		private ApplicationDbContext _context;
		private IConfiguration _config;
		public AccountController(ApplicationDbContext context, IConfiguration config) { 
			_context = context;
			_config = config;
		}

		[HttpPost("auth")]
		public async Task<IActionResult> Auth([FromBody]Account account)
		{
			//var exist = await _context.Accounts.FirstOrDefaultAsync(acc => acc.Username == account.username && acc.Password == acc.Password);

			//if(exist == null)
			//{
			//	return Ok(new
			//	{
			//		Code = 1,
			//		Message = "Sai mật khẩu hoặc tên đăng nhập!"
			//	});
			//}

			return Ok(new
			{
				Code = 0,
				access_token = GenerateAccessToken("admin") //account.username)
			});
		}

		private string GenerateAccessToken(string username)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var secretKey = GenerateRandomSecretKey(); // Generate a random secret key
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
			new Claim(ClaimTypes.NameIdentifier, username)
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		private byte[] GenerateRandomSecretKey()
		{
			// Generate a random key of sufficient size
			var key = new byte[32]; // 256 bits
			using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
			{
				rng.GetBytes(key);
			}
			return key;
		}
	}


	public class Account {
		public required string username { get; set; }	
		public required string password { get; set; }
	}
}
