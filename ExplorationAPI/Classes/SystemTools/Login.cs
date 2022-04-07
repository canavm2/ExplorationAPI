using System.Security.Cryptography;
using Users;


namespace LoginTools
{

    public static class Login
    {
        //public static UserDto CreatePasswordHash(UserDto userDto, out byte[] hash, out byte[] salt)
        //{
        //    using (var hmac = new HMACSHA512())
        //    {
        //        salt = hmac.Key;
        //        hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(userDto.Password));
        //        userDto.Password = string.Empty;
        //    }
        //    return userDto;
        //}

        //public static bool VerifyPassword(string username, string password, byte[] hash, byte[] salt)
        //{
        //    if (!_userCache.CheckforUser(username)) throw new Exception("user does not exist");
        //    using (var hmac = new HMACSHA512(salt))
        //    {
        //        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        //        return computedHash.SequenceEqual(hash);
        //    }
        //}
    }
}
