using ClothingBrandApp.Web.ViewModels.Admin.UserManagement;

namespace ClothingBrand.Services.Core.Admin.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserManagementIndexViewModel>> GetUserManagementBoardDataAsync(string userId);

    }
}
