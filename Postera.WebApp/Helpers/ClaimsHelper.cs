using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Postera.WebApp.Helpers
{
    public class ClaimsHelper
    {
        public static ClaimsPrincipal CreateTokenClaimsPrincipal(string token)
        {
            var claim = new Claim(ClaimTypes.NameIdentifier, token);
            var claims = new List<Claim> { claim };
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public static string GetTokenFromClaims(ClaimsPrincipal user)
        {
            var token = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (token == null)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token;
        }
    }
}