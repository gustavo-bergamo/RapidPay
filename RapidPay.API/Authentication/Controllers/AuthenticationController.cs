using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RapidPay.API.Authentication.Models;
using RapidPay.Domain.Authentication.Models;
using RapidPay.Domain.Authentication.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RapidPay.API.Authentication.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(ApplicationUserManager applicationUserManager, IConfiguration configuration, IAuthenticationService authenticationService)
        {
            _applicationUserManager = applicationUserManager;
            _configuration = configuration;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserCredential userCredential)
        {
            var authResult = await _authenticationService.TryAuthenticateAsync(userCredential.UserName, userCredential.Password);

            if (authResult.authenticated)
            {
                var token = await GenerateTokenAsync(authResult.user);
                return Ok(token);
            }

            return Unauthorized();
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user)
        {
            var claims = await _applicationUserManager.GetClaimsAsync(user);

            var key = _configuration["Jwt:Key"];
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                                      new SymmetricSecurityKey(keyBytes),
                                      SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
