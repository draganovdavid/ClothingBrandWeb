using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository categoyRepository;

        public CategoryService(ICategoryRepository categoyRepository)
        {
            this.categoyRepository = categoyRepository;
        }

        public async Task<IEnumerable<AddProductCatgoryDropDownModel>> GetAllCategoriesDropDownAsync()
        {
            IEnumerable<AddProductCatgoryDropDownModel> categoriesAsDropDown = await
                this.categoyRepository
                .GetAllAttached()
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