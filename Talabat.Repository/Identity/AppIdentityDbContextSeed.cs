using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "Ahmed Elaraby",
                    Email = "aelaraby999@gmail.com",
                    UserName = "aelaraby999",
                    PhoneNumber = "01069052505"
                };
                await userManager.CreateAsync(User, "P@ssw0rd");
            }
        }
    }
}
