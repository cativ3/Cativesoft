using Cativesoft.Security.Models;
using System;

namespace Cativesoft.Security.Helpers
{
    public interface ITokenHelper
    {
        TokenModel CreateToken(User user);
    }
}
