using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ControllerDeAcesso.Data;
using Microsoft.IdentityModel.Tokens;

namespace ControllerDeAcesso.Services
{
    public class TokenService
    {

        private IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserModel user)
        {
            Claim[] claims = new Claim[]{
                new Claim("username", user.UserName),
                new Claim("id", user.Id),
                new Claim(ClaimTypes.DateOfBirth, user.BirthDay.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SymmetricSecurityKey"]));
            var signingCredentials = new SigningCredentials (key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(10),
                claims: claims,
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }


    }
}