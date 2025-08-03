using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
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

        public async Task<IEnumerable<Category>> GetAllCategoriesDropDownAsync()
        {
            IEnumerable<Category> categoriesAsDropDown = await this.categoryRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(c => new Category()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categoriesAsDropDown;
        }
    }
}