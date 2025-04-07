using BeautyStore.API.Models;
using BeautyStore.Domain.Entities;
using BeautyStore.Domain.Interfaces.Service;
using BeautyStore.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BeautyStore.API.Controllers
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : ControllerBase
    {
        private readonly IVendedorService _vendedorService;
        private readonly JwtSettings _jwtSettings;

        public AuthController(IVendedorService vendedorService, IOptions<JwtSettings> jwtSettings)
        {
            _vendedorService = vendedorService;
            _jwtSettings = jwtSettings.Value;
        }

        /// <summary>
        ///     Realizar o login, com base no email e password e retornar o Jwt
        /// </summary>
        /// <param name="id">Id</param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)] 
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var usuario = await _vendedorService.BuscarVendedorPorNome(loginUser.Email);
            if (usuario == null)
            {
                return Unauthorized("Usuário ou senha incorretos.");
            }

            var token = GerarJwt(loginUser.Email);
            return Ok(new { Token = token });
        }

        private string GerarJwt(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Segredo);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Email, email) }),
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.Audiencia,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}