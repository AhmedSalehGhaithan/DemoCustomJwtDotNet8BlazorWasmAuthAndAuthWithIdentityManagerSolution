using SharedClassLibrary.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedClassLibrary.GenericModels
{
    public static class Generics
    {

        public static ClaimsPrincipal SetClaimPrincipal(UserSession model)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(
                new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier,model.Id!),
                    new Claim(ClaimTypes.Name,model.Name!),
                    new Claim(ClaimTypes.Email,model.Email!),
                    new Claim(ClaimTypes.Role,model.Role!),
                }, "JwtAuth"));
        }

        public static UserSession GetClaimsFromToken(string jwtToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);
            var claims = token.Claims;

            string ID = claims.First(_I_ => _I_.Type == ClaimTypes.NameIdentifier).Value;
            string Name = claims.First(_N_ => _N_.Type == ClaimTypes.Name).Value;
            string Email = claims.First(_E_ => _E_.Type == ClaimTypes.Email).Value;
            string Role = claims.First(_R_ => _R_.Type == ClaimTypes.Role).Value;

            return new UserSession(ID, Name, Email, Role);

        }

        public static JsonSerializerOptions JsonOptions()
        {
            return new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
            };
        }
        public static string SerializeObj<T>(T obj) => JsonSerializer.Serialize(obj, JsonOptions());
        public static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
        public static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;
        public static StringContent GenerateStringContent(string serializedObj) => new(serializedObj, Encoding.UTF8, "application/json");
        
    }
}
