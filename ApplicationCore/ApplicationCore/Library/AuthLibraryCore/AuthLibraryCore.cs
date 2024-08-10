using ApplicationCore.Utility.Common;
using ApplicationCore.Utility.Startup;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationCore.Library.AuthLibraryCore
{
    public class AuthLibraryCore
    {
        public static string GetBitStringEncryptHASH256WithKey(string encryptedData, string secretKey)
        {
            byte[] encryptedBytes = Encoding.UTF8.GetBytes(encryptedData);
            byte[] keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using (var hmac = new HMACSHA256(keyBytes))
            {
                byte[] signature = hmac.ComputeHash(encryptedBytes);
                string textEncode = BitConverter.ToString(signature);
                textEncode = textEncode.Replace("-", "").ToLower();
                return textEncode;
            }
        }
        public static string CreateJWTAfterLogin(string account)
        {
            string keyCreateJWT = GlobalConfigUtility.Config.Appsettings.JwtSecretKey;
            byte[] SecurityKeyBytes = ConvertUtility.GetBytes(keyCreateJWT);
            var SecurityKey = new SymmetricSecurityKey(SecurityKeyBytes);
            var signingCredentials = new SigningCredentials(SecurityKey, "HS256");
            var token = new JwtSecurityToken("TVTHAI", null, null, notBefore: null, expires: DateTime.UtcNow.AddHours(1), signingCredentials);
            token.Payload["account"] = account;
            token.Payload["createdDate"] = DateTime.Now;
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.WriteToken(token);
            var test = handler.ReadJwtToken(jwt);
            return $"Bearer {jwt}";
        }
    }
}
