using EmployMe.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployMe.Repositories
{
    public class TokenRepository:ITokenRepository
    { 
         private readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public string CreateJWTToken(Account account, List<string> roles)
        {
            //create claims
            var claims = new List<Claim>();
            //add claim type and value (Email)
            claims.Add(new Claim(ClaimTypes.Email, account.Email));
            //add role to claims
            foreach (var role in roles)
            {
                //add claim type and value (role)
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //-----------------------------------------------------------------------
            //create key (inject config to use it to access appsettings)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            //create credentials using above key 
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //create token using above claims and credentials
            var token = new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddHours(2), 
            signingCredentials: credentials);


            //initiates token using WriteToken()
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

