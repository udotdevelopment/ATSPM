using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AtspmApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AtspmApi.Repositories
{
    public class AuthRepository : IDisposable
    {
        private AuthContext authContext;
        private UserManager<IdentityUser> userManager;

        public AuthRepository()
        {
            authContext = new AuthContext();
            userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(authContext));
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            var atspmUser = await userManager.FindByNameAsync(userName);
            PasswordVerificationResult result = userManager.PasswordHasher.VerifyHashedPassword(atspmUser.PasswordHash, password);
            return await userManager.FindAsync(userName, password);
        }

        public void Dispose()
        {
            authContext.Dispose();
            userManager.Dispose();
        }

    }
}