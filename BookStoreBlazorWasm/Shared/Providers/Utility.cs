using System.Security.Claims;
using System.Text.Json;

namespace BookStoreBlazorWasm.Shared.Providers
{
    public class Utility
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = new List<Claim>();

            foreach (var kvp in keyValuePairs)
            {
                if (kvp.Key == ClaimTypes.Role)
                {
                    // Handle roles separately
                    if (kvp.Value is JsonElement roleElement)
                    {
                        // If the role value is a JSON array, add each role as a separate claim
                        if (roleElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var role in roleElement.EnumerateArray())
                            {
                                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                            }
                        }
                        else
                        {
                            // If the role value is a single string, add it as a single claim
                            claims.Add(new Claim(ClaimTypes.Role, roleElement.ToString()));
                        }
                    }
                }
                else
                {
                    // Add other claims as usual
                    claims.Add(new Claim(kvp.Key, kvp.Value.ToString()));
                }
            }

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}

