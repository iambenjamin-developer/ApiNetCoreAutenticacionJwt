using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ApiNetCoreAutenticacionJwt.Models;
using ApiNetCoreAutenticacionJwt.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ApiNetCoreAutenticacionJwt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpPost]
        public async Task<ActionResult<UserToken>> ValidarLogin([FromBody] LoginUser loginUser)
        {
            var servicio = new ServicioOracle();
            bool validacionOK = servicio.ValidarLogin(loginUser);
            List<string> roles = new List<string>() { "admin", "comercial", "personal" };


            if (validacionOK == true)
            {
                return BuildToken(loginUser, roles);
            }
            else
            {

                ModelState.AddModelError("Usuario/Clave", "Usuario o clave incorrectos");
                return BadRequest(ModelState);
            }

        }


        private UserToken BuildToken(LoginUser loginUser, IList<string> roles)
        {
            var claims = new List<Claim>
            {
        new Claim(JwtRegisteredClaimNames.UniqueName, loginUser.Usuario),
        new Claim(JwtRegisteredClaimNames.Email, loginUser.Usuario + "@cyre.com.ar"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tiempo de expiración del token. En nuestro caso lo hacemos de media hora
            var expiration = DateTime.UtcNow.AddMinutes(30);

            JwtSecurityToken token = new JwtSecurityToken(
               issuer: null,
               audience: null,
               claims: claims,
               expires: expiration,
               signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

    }
}