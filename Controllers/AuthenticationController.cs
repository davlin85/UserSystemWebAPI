using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Data;
using WebAPI.Models.Entities;
using WebAPI.Models.Input;
using WebAPI.Models.Interfaces;
using WebAPI.Models.Models;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;

        public AuthenticationController(AppDbContext appDbContext, IConfiguration configuration)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
        }


        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn(SignInModel model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest();
            }

            var admin = await _appDbContext.Admins.FirstOrDefaultAsync(x => x.Email == model.Email);

            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email);


            if (admin == null)
            {
                return BadRequest();
            }


            if (model.Email == admin.Email)
            {
                if (!admin.MatchPassword(model.Password))
                {
                    return BadRequest();
                }

                else
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim("id", admin.Id.ToString()),
                        new Claim(ClaimTypes.Email, admin.Email),
                        new Claim(ClaimTypes.Name, admin.Id.ToString()),
                        new Claim("code", _configuration.GetValue<string>("AdminAPIKey")),
                        new Claim("RolesPolicy", RolesPolicy.Admin.ToString())
                        }),

                        Expires = DateTime.UtcNow.AddMinutes(60),

                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"))),
                            SecurityAlgorithms.HmacSha512Signature)

                    };

                    var accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));


                    return Ok(new { accessToken });

                }

            }
            else if (model.Email == user.Email)
            {
                if (!user.MatchPassword(model.Password))
                {
                    return BadRequest();
                }

                else
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new Claim("id", user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Id.ToString()),
                        new Claim("code", _configuration.GetValue<string>("UserAPIKey")),
                        new Claim("RolesPolicy", RolesPolicy.User.ToString())
                        }),

                        Expires = DateTime.UtcNow.AddMinutes(60),

                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("SecretKey"))),
                            SecurityAlgorithms.HmacSha512Signature)
                        
                    };

                    var accessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));

                    return Ok( new { accessToken });
                }

            }
            else
            {
                return BadRequest();
            }
        
        }

        [HttpPost("SignUp")]
        public async Task<ActionResult<AdminModel>> CreateAdmin(AdminInput model)
        {
            if (await _appDbContext.Admins.AnyAsync(x => x.Email == model.Email))
                return BadRequest("Email already Exist!");

            var adminEntity = new AdminEntity(
                model.FirstName,
                model.LastName,
                model.Email);

            adminEntity.CreatePassword(model.Password);

            _appDbContext.Admins.Add(adminEntity);
            await _appDbContext.SaveChangesAsync();

            return Ok("New Admin created!");
        }
    }
}
