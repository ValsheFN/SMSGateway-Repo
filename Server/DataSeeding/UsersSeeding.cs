using Microsoft.AspNetCore.Identity;
using SMSGateway.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSGateway.Server.DataSeeding
{
    public class UsersSeeding
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersSeeding(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedData()
        {
            if (await _roleManager.FindByNameAsync("Super User") != null)
            {
                return;
            }
            else
            {
                var superUserRole = new IdentityRole { Name = "Super User" };
                await _roleManager.CreateAsync(superUserRole);

                var adminRole = new IdentityRole { Name = "Admin" };
                await _roleManager.CreateAsync(adminRole);

                var userRole = new IdentityRole { Name = "User" };
                await _roleManager.CreateAsync(userRole);
            }

            if (await _userManager.FindByNameAsync("Super User") != null)
            {
                return;
            }
            else
            {
                //Create user
                var superUser = new ApplicationUser
                {
                    Email = "filbert.nicholas93@gmail.com",
                    UserName = "filbert.nicholas93@gmail.com",
                    AdminApproval = true
                };

                await _userManager.CreateAsync(superUser, "P@ssw0rd.123");
                await _userManager.AddToRoleAsync(superUser, "Super User");
            }
        }
    }
}
