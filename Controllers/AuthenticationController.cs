using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
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
        public void SignIn([FromBody] string value)
        {
        }

        [HttpPost("SignUp")]
        public void SignUp([FromBody] string value)
        {
        }

    }
}
