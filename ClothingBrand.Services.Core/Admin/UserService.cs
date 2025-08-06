using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ClothingBrandApp.GCommon.ApplicationConstants;

namespace ClothingBrand.Services.Core.Admin
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IManagerRepository managerRepository;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IManagerRepository managerRepository)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.managerRepository = managerRepository;
        }

        public async Task<IEnumerable<UserManagementIndexViewModel>> GetUserManagementBoardDataAsync(string userId)
        {
            IEnumerable<UserManagementIndexViewModel> users = await this.userManager
                .Users
                .Where(u => u.Id.ToLower() != userId.ToLower())
                .Select(u => new UserManagementIndexViewModel
                {
                    Id = u.Id,
                    Email = u.Email!,
                    Roles = userManager.GetRolesAsync(u)
                        .GetAwaiter()
                        .GetResult()
                })
                .ToArrayAsync();

            return users;
        }

        public async Task<IEnumerable<string>> GetManagerEmailsAsync()
        {
            IEnumerable<string> managerEmails = await this.managerRepository
                .GetAllAttached()
                .Where(m => m.User.UserName != null)
                .Select(m => (string)m.User.UserName!)
                .ToArrayAsync();

            return managerEmails;
        }

        public async Task<bool> AssignUserToRoleAsync(RoleSelectionInputModel inputModel)
        {
            ApplicationUser? user = await this.userManager
                .FindByIdAsync(inputModel.UserId);

            if (user == null)
            {
                throw new ArgumentException("User does not exist!");
            }

            bool roleExists = await this.roleManager.RoleExistsAsync(inputModel.Role);
            if (!roleExists)
            {
                throw new ArgumentException("Selected role is not a valid role!");
            }

            if (inputModel.Role == ManagerRoleName)
            {
                var existingManager = await this.managerRepository
                    .GetAllAttached()
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(m => m.UserId == user.Id);

                if (existingManager != null)
                {
                    if (existingManager.IsDeleted)
                    {
                        existingManager.IsDeleted = false;
                        await this.managerRepository.UpdateAsync(existingManager);
                    }
                }
                else
                {
                    var newManager = new Manager
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        IsDeleted = false
                    };

                    await this.managerRepository.AddAsync(newManager);
                }
            }

            try
            {
                await this.userManager.AddToRoleAsync(user, inputModel.Role);

                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    "Unexpected error occurred while adding the user to role! Please try again later!",
                    innerException: e);
            }
        }

        public async Task<bool> RemoveUserFromRoleAsync(RoleSelectionInputModel inputModel)
        {
            ApplicationUser? user = await this.userManager.FindByIdAsync(inputModel.UserId);

            if (user == null)
            {
                throw new ArgumentException("User does not exist!");
            }

            bool roleExists = await this.roleManager.RoleExistsAsync(inputModel.Role);
            if (!roleExists)
            {
                throw new ArgumentException("Selected role is not a valid role!");
            }

            if (!await this.userManager.IsInRoleAsync(user, inputModel.Role))
            {
                throw new InvalidOperationException("User is not in the specified role.");
            }

            if (inputModel.Role == ManagerRoleName)
            {
                Manager? existManager = await this.managerRepository
                    .SingleOrDefaultAsync(m => m.UserId == inputModel.UserId);

                if (existManager != null)
                {
                    if (existManager.IsDeleted == false)
                    {
                        existManager.IsDeleted = true;
                        await this.managerRepository.UpdateAsync(existManager);
                    }
                }
            }

            try
            {
                var result = await this.userManager.RemoveFromRoleAsync(user, inputModel.Role);
                return result.Succeeded;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Unexpected error occurred while removing the user from role.", e);
            }
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            ApplicationUser? user = await this.userManager
                .FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            bool isUserAdmin = await this.userManager.IsInRoleAsync(user, "Admin");

            if (!isUserAdmin)
            {
                throw new InvalidOperationException("Cannot delete Admin users.");
            }

            try
            {
                var result = await this.userManager.DeleteAsync(user);

                return result.Succeeded;
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Error deleting user.", e);
            }
        }
    }
}