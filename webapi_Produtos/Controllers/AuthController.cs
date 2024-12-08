using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using webapi_Produtos.Models.UserLogin;

namespace webapi_Produtos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin login)
        {
            if (login.Username == "ger" && login.Password == "ger")
            {
                var token = GerarTokenJWT(login.Username);
                return Ok(new { Token = token });
            }
            else if (login.Username == "func" && login.Password == "func")
            {
                var token = GerarTokenJWT(login.Username);
                return Ok(new { Token = token });
            }

            return Unauthorized(new { Message = "Credenciais inválidas.", Codigo = 001 });
        }


        private string GerarTokenJWT(string username)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Role, username.ToLower())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("8dd246bea4ed33a8f5b2ac02a0f3b6cc0e2d09ea25220baf0678f0a5e7198f86554147d0ffe4072bc0423d0ac9cfa24817b8e809479f7cc14a3fe2f75313082ef07ed9f5e9f34483a95e655628ad9e23b40785b605618b5dadcdb62a086c4195091592ecc3a29483f7c75e0c74bb1b2b999a7006133f8695c94b4b14f5b9aabe8f8ce59e4e2162cc832ef9ad65048eaf69ebfa8b080324ccf8bbb11e1b8cff8941e1b7ef70f61a24c319a1dc9ed5b93f27623d272d3ee6975b0a59902bfd7a9e9f433a53490e2dcb6b0e767e723a8c531e7192f98dfffa73a29e0281a3dfd9ed68c3a8282b1093cf25f96a982ac780771e706d49012896badbbf3a35f1c65dd0"));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                issuer: "api-autenticacao",
                audience: "api-produtos",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
