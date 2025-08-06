using ClothingBrand.Services.Core.Admin.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.UserManagement;
using Microsoft.AspNetCore.Mvc;
using static ClothingBrandApp.GCommon.ApplicationConstants;

namespace ClothingBrandApp.Web.Areas.Admin.Controllers
{
    public class UserManagementController : BaseAdminController
    {
        private readonly IUserService userService;

        public UserManagementController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserManagementIndexViewModel> allUsers = await this.userService
                .GetUserManagementBoardDataAsync(this.GetUserId()!);

            return View(allUsers);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleSelectionInputModel inputModel)
        {
            try
            {
                await this.userService
                    .AssignUserToRoleAsync(inputModel);
                TempData[SuccessMessageKey] = "User assigned to role successfully!";
            }
            catch (Exception e)
            {
                TempData[ErrorMessageKey] = e.Message;
            }

            return this.RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRole(RoleSelectionInputModel inputModel)
        {
            try
            {
                bool removed = await this.userService.RemoveUserFromRoleAsync(inputModel);

                if (removed)
                {
                    TempData[SuccessMessageKey] = "Role removed successfully!";
                }
                else
                {
                    TempData[ErrorMessageKey] = "Could not remove role from user.";
                }
            }
            catch (Exception ex)
            {
                TempData[ErrorMessageKey] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                bool deleted = await this.userService.DeleteUserAsync(userId);

                if (deleted)
                {
                    TempData[SuccessMessageKey] = "User deleted successfully!";
                }
                else
                {
                    TempData[ErrorMessageKey] = "User could not be deleted.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData[ErrorMessageKey] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
