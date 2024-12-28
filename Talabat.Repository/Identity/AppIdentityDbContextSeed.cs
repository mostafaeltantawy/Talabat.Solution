using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Enitities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Mostafa Eltantawy",
                    Email = "eltantawymostafa2@gmail.com",
                    UserName = "eltantawymostafa2",
                    PhoneNumber = "012345678911"
                };
                await userManager.CreateAsync(User, "P@ssword1");
            }
 
        }
    }
}
