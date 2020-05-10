using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Postera.WebApp.Helpers
{
    public class ClaimsHelper
    {
        public static ClaimsPrincipal CreateTokenClaimsPrincipal(string token)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
            
            var tokenClaim = new Claim(ClaimTypes.Authentication, token);
            var claims = jwtSecurityToken.Claims.ToList();
            claims.Add(tokenClaim);

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public static string GetTokenFromClaims(ClaimsPrincipal user)
        {
            var token = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Authentication)?.Value;
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token;
        }
    }
}