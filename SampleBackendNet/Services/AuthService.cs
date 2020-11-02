using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SampleBackendNet.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SampleBackendNet.Services
{
    // Authentication on your backend (in this sample, it's also JWT auth)
    public interface IAuthService
    {
        AuthResponse Authenticate(AuthRequest model, HttpContext httpContext);
        IEnumerable<User> GetAll();
        User GetByIdentity(string identity);
    }
    public class AuthService : IAuthService
    {
        // Test list of users
        private List<User> _users = new List<User>
        {
            new User { Identity = "alice@some.mail", OtherInfo = "Alice", Password = "test" },
            new User { Identity = "bob@some.mail", OtherInfo = "Bob", Password = "test" }
        };

        private readonly IConfiguration _configuration;


        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthResponse Authenticate(AuthRequest model, HttpContext httpContext)
        {
            var user = _users.SingleOrDefault(x => x.Identity == model.Identity && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so create jwt
            var claims = new List<Claim>
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Identity),
                    new Claim(ClaimTypes.Name, user.OtherInfo)
                    };

            var claimsIdentity = new ClaimsIdentity(claims);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponse(user, jwtToken);
        }


        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetByIdentity(string identity)
        {
            return _users.FirstOrDefault(x => x.Identity == identity);
        }
    }
}
