using Core.Repository.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Security
{
    public class TokenBuilder : ITokenBuilder
    {
        private readonly string _secret;
        private readonly string _expDate;

        public TokenBuilder(IConfiguration config)
        {
            _secret = config.GetSection("JwtConfig").GetSection("secret").Value;
            _expDate = config.GetSection("JwtConfig").GetSection("expirationInMinutes").Value;
        }

        public string BuildToken(UserClaim userClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userClaims.Name),
                    new Claim("UserId", userClaims.UserId.ToString()),
                    new Claim("BranchId", userClaims.BranchId.ToString()),
                    new Claim("BranchCode", userClaims.BranchCode),
                    new Claim("RoleTypeId", userClaims.RoleTypeId.ToString()),
                    new Claim("RegionId", userClaims.RegionId.ToString()),
                    new Claim("RoleIds", string.Join(",", userClaims.RoleIds)),
                }),
                Expires = DateTime.Now.AddMinutes(double.Parse(_expDate)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
