using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Data;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<AddProductCatgoryDropDownModel>> GetAllCategoriesDropDownAsync()
        {
            IEnumerable<AddProductCatgoryDropDownModel> categoriesAsDropDown = await
                this.dbContext
                .Categories
                .AsNoTracking()
                .Select(c => new AddProductCatgoryDropDownModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categoriesAsDropDown;
        }
    }
}