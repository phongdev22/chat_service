using chat_service.MyDbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace chat_service.Controllers
{	
	[ApiController]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private ApplicationDbContext _context;
		private IConfiguration _config;
		private UserManager<IdentityUser> _userManager;
		private RoleManager<IdentityRole> _roleManager;

		public AccountController(ApplicationDbContext context,
								 UserManager<IdentityUser> userManager,
								 RoleManager<IdentityRole> roleManager,
								 IConfiguration config) { 
			_context = context;
			_config = config;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		[HttpPost("auth")]
		[AllowAnonymous]
		public async Task<IActionResult> Auth([FromBody]Account account)
		{
			try
			{
				var user = await _userManager.FindByNameAsync(account.username);

				if(user!= null && await _userManager.CheckPasswordAsync(user, account.password.Trim()))
				{
					var userRoles = await _userManager.GetRolesAsync(user);
					//
					var authClaims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, account.username),
						new Claim(ClaimTypes.Role, "Admin"),
					};
					//
					foreach (var userRole in userRoles)
					{
						authClaims.Add(new Claim(ClaimTypes.Role, userRole));
					}
					// Sign Indentity
					var identityClaims = new ClaimsIdentity(authClaims, "Identity");
					var token = GenerateAccessToken(identityClaims);

					return Ok(new
					{
						code = 0,
						accessToken = token
					});
				}else
				{
					return Ok(new {Code=0, Message= "Invalid credentials" });
				}

			}catch (Exception ex)
			{
				return Ok(new {Code = 1, Message = ex.Message});
			}
		}

		[Authorize]
		[HttpPost("create")]
		public async Task<IActionResult> Create([FromBody]Account account)
		{
			try
			{
				Console.WriteLine(User.Identity.Name);
				var result = await _userManager.CreateAsync(new IdentityUser()
				{
					UserName = account.username,
				}, account.password);

				if (result.Succeeded)
				{
					return Ok(new
					{
						Code = 0,
						Message = "Create account success!",
					});
				}else
				{
					return Ok(new
					{
						Code = 1 ,
						Errors = result.Errors
					});
				}

			}
			catch (Exception ex)
			{
				return Ok(new
				{
					Code = 0,
					Errors = ex.Message
				});
			}
		}

		private string GenerateAccessToken(ClaimsIdentity identity)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			var key = Encoding.UTF8.GetBytes("your_secret_key_van_cong_nguyen_phong");

			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = identity,
				Expires = DateTime.UtcNow.AddHours(24),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}

	public class Account {
		public required string username { get; set; }	
		public required string password { get; set; }
	}
}
