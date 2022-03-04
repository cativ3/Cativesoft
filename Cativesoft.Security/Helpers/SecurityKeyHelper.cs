using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cativesoft.Security.Helpers
{
    public static class SecurityKeyHelper
    {
        public static SecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
        }
    }
}
