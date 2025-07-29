using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClothingBrand.Services.Core
{
    public class ShopService : IShopService
    {
        private readonly IShopRepository shopRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IWarehouseRepository warehouseRepository;
        private readonly UserManager<ApplicationUser> userManager;
        
        public ShopService(IShopRepository shopRepository, ICategoryRepository categoryRepository, IWarehouseRepository warehouseRepository, 
            UserManager<ApplicationUser> userManager)
        {
            this.shopRepository = shopRepository;
            this.userManager = userManager;
            this.categoryRepository = categoryRepository;
            this.warehouseRepository = warehouseRepository;
        }

        public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync()
        {
            IEnumerable<ProductIndexViewModel> allProducts = await this.shopRepository
                .GetAllAttached()
                .AsNoTracking()
                .Select(p => new ProductIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    InStock = p.InStock
                })
                .ToListAsync();

            // TODO: Add noImage if the image is null
            //foreach (ProductIndexViewModel movie in allProducts)
            //{
            //    if (String.IsNullOrEmpty(movie.ImageUrl))
            //    {
            //        movie.ImageUrl = $"/images/{NoImageUrl}";
            //    }
            //}

            return allProducts;
        }

        public async Task AddProductAsync(string userId, ProductFormInputModel inputModel)
        {
            ApplicationUser? user = await this.userManager.FindByIdAsync(userId);
            Category? category = await this.categoryRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.CategoryId);
            Warehouse? warehouse = await this.warehouseRepository
                .FirstOrDefaultAsync(w => w.Name.ToLower() == inputModel.WarehouseName.ToLower());
            int genderId = GetGenderId(inputModel.Gender);

            if (user != null && category != null && genderId != 0 && inputModel.Price.HasValue &&
                warehouse != null)
            {
                var newProduct = new Product
                {
                    Id = Guid.NewGuid(),
                    Name = inputModel.Name,
                    Price = inputModel.Price.Value,
                    Description = inputModel.Description,
                    Size = inputModel.Size,
                    ImageUrl = inputModel.ImageUrl,
                    InStock = inputModel.InStock,
                    WarehouseId = warehouse.Id,
                    CategoryId = category.Id,
                    GenderId = genderId,
                    IsDeleted = false
                };

                await this.shopRepository.AddAsync(newProduct);
            }
        }

        public async Task<ProductDetailsViewModel?> GetProductDetailsByIdAsync(Guid? id)
        {
            return await this.shopRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Size = p.Size,
                    ImageUrl = p.ImageUrl,
                    InStock = p.InStock,
                    CategoryName = p.Category.Name,
                    GenderName = p.Gender.Name,
                })
                .SingleOrDefaultAsync();
        }

        public async Task<ProductFormInputModel?> GetProductForEditingAsync(Guid? productId)
        {
            if (productId == null)
            {
                return null;
            }

            return await this.shopRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(p => p.Id == productId)
                .Select(p => new ProductFormInputModel
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    Gender = p.Gender.Name,
                    Size = p.Size,
                    Description = p.Description,
                    ImageUrl = p.ImageUrl,
                    InStock = p.InStock
                })
                .SingleOrDefaultAsync();
        }

        public async Task<bool> EditProductAsync(string userId, ProductFormInputModel inputModel)
        {
            bool result = false;
            if (!Guid.TryParse(inputModel.Id, out var productId) || !inputModel.Price.HasValue)
            {
                return result;
            }
            Product? editableProduct = await this.shopRepository
                .GetByIdAsync(productId);
            Category? category = await this.categoryRepository
                .GetByIdAsync(inputModel.CategoryId);
            Warehouse? warehouse = await this.warehouseRepository
                .FirstOrDefaultAsync(w => w.Name.ToLower() == inputModel.WarehouseName.ToLower());
            int genderId = GetGenderId(inputModel.Gender);

            if (editableProduct == null || category == null || genderId == 0 || warehouse == null)
            {
                return false;
            }

            editableProduct.Name = inputModel.Name;
            editableProduct.Price = inputModel.Price.Value;
            editableProduct.Description = inputModel.Description;
            editableProduct.Size = inputModel.Size;
            editableProduct.ImageUrl = inputModel.ImageUrl;
            editableProduct.InStock = inputModel.InStock;
            editableProduct.CategoryId = category.Id;
            editableProduct.GenderId = genderId;
            editableProduct.WarehouseId = warehouse.Id;

            result = await this.shopRepository.UpdateAsync(editableProduct);

            return result;
        }

        public async Task<ProductDeleteInputModel?> GetProductForDeleteAsync(Guid? productId)
        {
            ProductDeleteInputModel? deleteModel = null;

            if (productId != null)
            {
                Product? deleteProductModel = await this.shopRepository
                    .GetAllAttached()
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == productId);

                if (deleteProductModel != null)
                {
                    deleteModel = new ProductDeleteInputModel()
                    {
                        Id = deleteProductModel.Id,
                        Name = deleteProductModel.Name,
                        ImageUrl = deleteProductModel.ImageUrl,
                    };
                }
            }

            return deleteModel;
        }

        public async Task<bool> SoftDeleteAsync(ProductDeleteInputModel inputModel)
        {
            bool result = false;
            Product? deletedProduct = await this.shopRepository
                .GetByIdAsync(inputModel.Id);

            if (deletedProduct == null)
            {
                return result;
            }

            // Soft Delete <=> Edit of IsDeleted property
            result = await this.shopRepository.DeleteAsync(deletedProduct);

            return result;
        }


        // Extended mehods
        public async Task<IEnumerable<ProductIndexViewModel>> GetProductsByGenderAsync(string genderName)
        {
            var allProductsForMen = await this.shopRepository
                .GetAllAttached()
                .AsNoTracking()
                .Where(p => p.Gender.Name == genderName)
                .Select(p => new ProductIndexViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    Price = p.Price,
                    InStock = p.InStock
                })
                .ToListAsync();

            return allProductsForMen;
        }

        private int GetGenderId(string genderName)
        {
            return genderName switch
            {
                "Men" => 1,
                "Women" => 2,
                "Kids" => 3,
                _ => 0
            };
        }
    }
}