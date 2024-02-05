using ElderCare_Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Ultils
{
    public static class GenerateJWTString
    {

        public static string GenerateJsonWebToken(this Account account, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim("Id" ,account.AccountId.ToString()),
            new Claim(ClaimTypes.Name, account.Username),
            new Claim(ClaimTypes.Role, "Customer"),
        };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: now.AddHours(1),
                issuer: secretKey,
                audience: secretKey,
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GenerateJsonWebTokenForStaff(this Account account, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim("Id" ,account.AccountId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, account.Username),
            new Claim(ClaimTypes.Role, "Staff"),
        };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: now.AddHours(1),
                issuer: secretKey,
                audience: secretKey,
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GenerateJsonWebTokenForCarer(this Account account, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
             new Claim(ClaimTypes.SerialNumber ,account.AccountId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, account.Username),
            new Claim(ClaimTypes.Role, "Carer"),
        };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: now.AddHours(1),
                issuer: secretKey,
                audience: secretKey,
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public static string GenerateJsonWebTokenForAdmin(this string email, string secretKey, DateTime now)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(ClaimTypes.Email ,email),
            new Claim(ClaimTypes.Role, "Admin"),
        };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: now.AddHours(1),
                issuer: secretKey,
                audience: secretKey,
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
       
    }
}
