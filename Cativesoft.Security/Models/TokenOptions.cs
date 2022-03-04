using System.Collections.Generic;

namespace Cativesoft.Security.Models
{
    public class TokenOptions
    {
        public string Issuer { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
        public List<string> Audience { get; set; }
    }
}
