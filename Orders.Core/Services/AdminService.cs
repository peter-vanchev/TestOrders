using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Orders.Core.Contracts;
using Orders.Core.Models;
using Orders.Infrastructure.Data.Models;

namespace Orders.Core.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminService(
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<List<IdentityRole>> GetRole()                                                     
        {
            var roles = await roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<bool> AddRole(string roleName)
        {
            var suxess = false;
            if (roleName != null)
            {
                var test = await roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
                suxess = true;
            }

            return suxess;
        }

        public async Task<List<UserRolesViewModel>> GetUserRoles()
        {
            var users = await userManager.Users.ToListAsync();


            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return userRolesViewModel;
        }

        public async Task<List<UserRolesViewModel>> GetUserRoles(string role)
        {
            var users = await userManager.Users.ToListAsync();


            var userRolesViewModel = new List<UserRolesViewModel>();

            foreach (ApplicationUser user in users)
            {
                var thisViewModel = new UserRolesViewModel();
                thisViewModel.UserId = user.Id;
                thisViewModel.Email = user.Email;
                thisViewModel.Roles = await GetUserRoles(user);
                userRolesViewModel.Add(thisViewModel);
            }

            return userRolesViewModel;
        }

        public async Task<List<ManageUserRolesViewModel>> AddRoleToUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {                              
                return null;
            }

            var model = new List<ManageUserRolesViewModel>();

            foreach (var role in roleManager.Roles)
            {
                var userRolesViewModel = new ManageUserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.Selected = true;
                }
                else
                {
                    userRolesViewModel.Selected = false;
                }

                model.Add(userRolesViewModel);
            }
            return model;
        }

        public async Task<(bool, List<ManageUserRolesViewModel>)> ManageRole(List<ManageUserRolesViewModel> model, string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return (false, model);
            }

            var roles = await userManager.GetRolesAsync(user);

            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                return (false, model);
            }

            result = await userManager.AddToRolesAsync(user, model.Where(x => x.Selected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                return (false, model);
            }

            return (true, model);
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await userManager.GetRolesAsync(user));
        }

        public async Task<bool> Seed() 
        {
            var usersNumber = userManager.Users;
            if (usersNumber.Count() <= 1)
            {
                var admin = await roleManager.CreateAsync(new IdentityRole("Admin"));
                var manager = await roleManager.CreateAsync(new IdentityRole("Manager"));
                var restaurant = await roleManager.CreateAsync(new IdentityRole("Restaurant"));
                var driver = await roleManager.CreateAsync(new IdentityRole("Driver"));

                var userAdmin = await userManager.Users
                .Where(x => x.Email == "Ravinabg@abv.bg")
                .FirstOrDefaultAsync();
                var result = userManager.AddToRoleAsync(userAdmin, "Admin");

                if ((admin.Succeeded && manager.Succeeded && restaurant.Succeeded && driver.Succeeded && restaurant.Succeeded))
                {
                    return true;
                }                  
            };                              

            return false;
        }
    }
}
