using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Category;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryService(ICategoryRepository categoyRepository)
        {
            this.categoryRepository = categoyRepository;
        }

        public async Task<IEnumerable<AllCategoriesDropDownViewModel>> GetAllCategoriesDropDownAsync()
        {
            IEnumerable<AllCategoriesDropDownViewModel> categoriesAsDropDown = await this.categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(c => new AllCategoriesDropDownViewModel()
                {
                    Name = c.Name
                })
                .ToListAsync();

            return categoriesAsDropDown;
        }
    }
}