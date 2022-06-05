using ExplorationAPI.Services.UserServices;
using FileTools;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Users;

namespace ExplorationAPI.Services.LoginService
{
    //This is all used in the Login controllers (loging and admin)
    public interface ILoginService
    {
        public void CreatePasswordHash(UserDto userDto, out byte[] hash, out byte[] salt);
        public bool VerifyPassword(string username, string password, byte[] hash, byte[] salt, IUserCache userCache);
        public string CreateToken(User user, IFileTool fileTool, Boolean admin = false);
    }
    public class LoginService : ILoginService
    {
        public void CreatePasswordHash(UserDto userDto, out byte[] hash, out byte[] salt)
        {
            using (var hmac = new HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userDto.Password));
                userDto.Password = string.Empty;
            }
        }
        public bool VerifyPassword(string username, string password, byte[] hash, byte[] salt, IUserCache userCache)
        {
            if (!userCache.CheckforUser(username)) throw new Exception("user does not exist");
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(hash);
            }
        }
        public string CreateToken(User user, IFileTool fileTool, Boolean admin = false)
        {
            string role = "Player";
            if (admin) role = "Admin";
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, role)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(fileTool.LoginKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
    public class UserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    public class AdminDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AdminCheck { get; set; } = string.Empty;
    }
}
