using Cativesoft.Security.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Cativesoft.Security.Helpers
{
    public class JwtHelper : ITokenHelper
    {
        private readonly Models.TokenOptions _tokenOptions;
        private readonly UserManager<User> _userManager;

        public JwtHelper(IOptions<Models.TokenOptions> tokenOptions, UserManager<User> userManager)
        {
            _tokenOptions = tokenOptions.Value;
            _userManager = userManager;
        }

        public TokenModel CreateToken(User user)
        {
            var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
            var refreshTokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.RefreshTokenExpiration);

            var securityKey = SecurityKeyHelper.GetSymmetricSecurityKey(_tokenOptions.SecurityKey);

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _tokenOptions.Issuer,
                    expires: accessTokenExpiration,
                    notBefore: DateTime.Now,
                    claims: GetClaims(user, _tokenOptions.Audience),
                    signingCredentials: signingCredentials
                );

            var tokenHandler = new JwtSecurityTokenHandler();

            var accessToken = tokenHandler.WriteToken(token);

            var refreshToken = CreateRefreshToken();

            var tokenModel = new TokenModel
            {
                AccessToken = accessToken,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration
            };

            return tokenModel;
        }


        private IEnumerable<Claim> GetClaims(User user, List<string> audiences)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = _userManager.GetRolesAsync(user).Result;

            claims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            claims.AddRange(audiences.Select(audience => new Claim(JwtRegisteredClaimNames.Aud, audience)));

            return claims;

        }

        private string CreateRefreshToken()
        {
            return Guid.NewGuid().ToString() + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffffff");
        }
    }
}
