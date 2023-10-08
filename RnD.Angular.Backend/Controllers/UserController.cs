using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RnD.Angular.Backend.Contracts;
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
        private readonly IRefreshToken _refreshToken;

        public UserController(LearnDbContext learnDbContext, IOptions<JWTConfigModel> options, IRefreshToken refreshToken)
        {
            _learnDbContext = learnDbContext;
            _jwtConfigModel = options.Value;
            _refreshToken = refreshToken;
        }

        [NonAction]
        public TokenResponseModel Authenticate(string username, Claim[] claims)
        {
            TokenResponseModel responseModel = new TokenResponseModel();

            var tokenKey = Encoding.UTF8.GetBytes(_jwtConfigModel.SecurityKey);
            var tokenHandler = new JwtSecurityToken(
                claims: claims, expires: DateTime.Now.AddMinutes(2), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
                );
            responseModel.JWTToken = new JwtSecurityTokenHandler().WriteToken(tokenHandler);

            responseModel.RefreshToken = _refreshToken.GenerateToken(username);

            return responseModel;
        }

        [Route("Authenticate")]
        [HttpPost]
        public IActionResult Authentication([FromBody] UserCredentialModel userCredentialModel)
        {

            TokenResponseModel tokenResponseModel = new TokenResponseModel();

            var _user = _learnDbContext.TblUsers.FirstOrDefault(opt => opt.Userid == userCredentialModel.Username && opt.Password == userCredentialModel.Password);

            if (_user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_jwtConfigModel.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, _user.Userid),
                    new Claim(ClaimTypes.Role, _user.Role)
                }),
                Expires = DateTime.Now.AddMinutes(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256)
            };

            var tmpToken = tokenHandler.CreateToken(tokenDescriptor);
            string finalToken = tokenHandler.WriteToken(tmpToken);

            tokenResponseModel.JWTToken = finalToken;
            tokenResponseModel.RefreshToken = _refreshToken.GenerateToken(userCredentialModel.Username);

            return Ok(tokenResponseModel);
        }

        [Route("RefreshToken")]
        [HttpPost]
        public IActionResult RefreshToken([FromBody] TokenResponseModel tokenResponseModel)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(tokenResponseModel.JWTToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfigModel.SecurityKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out securityToken);

            var _token = securityToken as JwtSecurityToken;

            if (_token != null && !_token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return Unauthorized();
            }

            var username = principal.Identity.Name;
            var check = _learnDbContext.TblRefreshtokens.FirstOrDefault(opt => opt.UserId == username && opt.RefreshToken == tokenResponseModel.RefreshToken);

            if (check == null)
            {
                return Unauthorized();
            }

            TokenResponseModel _tokenResult = Authenticate(username, principal.Claims.ToArray());

            return Ok(_tokenResult);
        }
    }
}
