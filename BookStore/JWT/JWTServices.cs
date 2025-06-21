using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BookStore.JWT
{
    public class JWTServices
    {
        private string secureKey = "super_secure_key_32_chars_long_1234567890";
        public JWTServices() { }

        public string Generate(int id)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload(id.ToString(),null,null,null,DateTime.Today.AddDays(1));
            var securtyToken = new JwtSecurityToken(header, payload);
            return new JwtSecurityTokenHandler().WriteToken(securtyToken);
        }

        public JwtSecurityToken Verify(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secureKey);
            tokenHandler.ValidateToken(jwt,new TokenValidationParameters 
            { 
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
            }
            ,out SecurityToken validatedToken);
            return (JwtSecurityToken)validatedToken;

        }

        public int? GetUserIdFromToken(HttpContext context)
        {
            try
            {
                var jwt = context.Request.Cookies["jwt"];
                if (string.IsNullOrEmpty(jwt)) return null;

                var token = Verify(jwt);
                return int.Parse(token.Issuer);
            }
            catch
            {
                return null;
            }
        }

    }
}
