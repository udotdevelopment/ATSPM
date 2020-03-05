using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using AtspmApi.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AtspmApi
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated(); // 
            }

            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (AuthRepository authRepo = new AuthRepository())
            {
                IdentityUser user = await authRepo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                bool roleExist = false;
                var rolesTechnicalNamesUser = new List<string>();
                if (user.Roles != null)
                {
                    rolesTechnicalNamesUser = user.Roles.Select(x => x.RoleId).ToList();
                    string ApiRoleSetting = ConfigurationManager.AppSettings["ApiRole"];
                    foreach (var role in rolesTechnicalNamesUser)
                    {
                        if (role.Contains(ApiRoleSetting))
                        {
                            roleExist = true;
                            break;
                        }
                    }
                }

                if (!roleExist)
                {
                    context.SetError("invalid_grant", "The user does not have API access.");
                    return;
                }
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            // Optional : You can add a role based claim by uncommenting the line below.
            // identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));

            context.Validated(identity);

            //var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            //if (context.UserName == "admin" && context.Password == "admin")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "admin"));
            //    identity.AddClaim(new Claim("username", "admin"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "UDOT Admin"));
            //    context.Validated(identity);
            //}
            //else if (context.UserName == "userSpm" && context.Password == "UotApi")
            //{
            //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            //    identity.AddClaim(new Claim("username", "user"));
            //    identity.AddClaim(new Claim(ClaimTypes.Name, "Udot Atspm Api User"));
            //    context.Validated(identity);
            //}
            //else
            //{
            //    context.SetError("invalid_grant", "Provided username and password is incorrect");
            //    return;
            //}
        }
    }
}
