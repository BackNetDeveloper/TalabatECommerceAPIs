using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Ahmed",
                    Email = "Ahmed@gmail.com",
                    UserName = "Ahmedatwan",
                    Address = new Address()
                    {
                        FirstName = "Ahmed",
                        LastName = "Atwan",
                        Street="10 St",
                        City = "Mallawi",
                        State = "Minya",
                        ZipCode="19951229"
                    }
                };
                 await userManager.CreateAsync(user,"12345@Atwan");
            }
        }

    }
}
