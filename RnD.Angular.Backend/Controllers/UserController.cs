using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RnD.Angular.Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RnD.Angular.Backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LearnDbContext _learnDbContext;
        private readonly JWTConfigModel _jwtConfigModel;
        public UserController(LearnDbContext learnDbContext, IOptions<JWTConfigModel> options)
        {
            _learnDbContext = learnDbContext;
            _jwtConfigModel = options.Value;
        }

        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authentication([FromBody] UserCredentialModel userCredentialModel)
        {
            var _user = _learnDbContext.TblUsers.FirstOrDefault(opt => opt.Userid == userCredentialModel.Username && opt.Password == userCredentialModel.Password);

            if (_user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_jwtConfigModel.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, _user.Userid)
                }),
                Expires = DateTime.Now.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };

            var tmpToken = tokenHandler.CreateToken(tokenDescriptor);
            string finalToken = tokenHandler.WriteToken(tmpToken);

            return Ok(finalToken);
        }
    }
}
