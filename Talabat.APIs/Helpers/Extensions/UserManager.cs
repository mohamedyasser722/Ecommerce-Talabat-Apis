﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Helpers.Extensions
{
    public static class UserManager
    {
        public static async Task<AppUser> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager , ClaimsPrincipal currentUser)
        {
            var email = currentUser.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }
    }
}
