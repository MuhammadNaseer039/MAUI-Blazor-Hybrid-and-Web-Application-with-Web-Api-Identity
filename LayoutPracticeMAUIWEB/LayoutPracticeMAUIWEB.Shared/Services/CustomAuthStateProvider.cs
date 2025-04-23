using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LayoutPracticeMAUIWEB.Shared.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ITokenStorageService tokenStorage;
        public CustomAuthStateProvider(ITokenStorageService tokenStorage)
        {
            this.tokenStorage = tokenStorage;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            

            //var tokenexpiry = await tokenStorage.GetTokenExpiryAsync();

            

            try
            {
                var token = await tokenStorage.GetTokenAsync();
                if (token == null)
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
                var tokenhandler = new JwtSecurityTokenHandler();

                var Jwttoken = tokenhandler.ReadJwtToken(token);

                var exp = Jwttoken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;

                if(long.TryParse(exp,out var expUnix))
                {
                    var tokenexpiry = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                    if (tokenexpiry < DateTime.UtcNow)
                    {
                        await tokenStorage.RemoveTokenAsync();
                        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    }
                }
                var identity = new ClaimsIdentity(Jwttoken.Claims, "Jwt");

                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

        }

        public void NotifyUserAuthenticationAsync(string token)
        {

            var tokenhandler = new JwtSecurityTokenHandler();

            var jwttoken = tokenhandler.ReadJwtToken(token);


            var identity = new ClaimsIdentity(jwttoken.Claims, "Jwt");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void NotifyUserLogout()
        {
            var anonymous = new ClaimsPrincipal(new ClaimsIdentity());

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymous)));
        }
    }
}
