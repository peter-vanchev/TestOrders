﻿using Microsoft.AspNetCore.Identity;
using TestOrders.Models;

namespace TestOrders.Contracts
{
    public interface IAdminService
    {
        public Task<List<IdentityRole>> GetRole();

        public Task<bool> AddRole(string roleName);

        public Task<List<UserRolesViewModel>> GetUserRoles();

        public Task<List<ManageUserRolesViewModel>> AddRoleToUser(string userId);

        public Task<(bool, List<ManageUserRolesViewModel>)> ManageRole(List<ManageUserRolesViewModel> model, string userId);
    }
}
