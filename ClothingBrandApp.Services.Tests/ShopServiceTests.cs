using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core;
using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.ViewModels.Admin.ProductManagement;
using MockQueryable;
using Moq;

namespace ClothingBrandApp.Services.Tests
{
    [TestFixture]
    public class ShopServiceTests
    {
        private Mock<IShopRepository> shopRepositoryMock;
        private Mock<ICategoryRepository> categoryRepositoryMock;
        private Mock<IWarehouseRepository> warehouseRepositoryMock;
        private IShopService shopService;

        [SetUp]
        public void Setup()
        {
            shopRepositoryMock = new Mock<IShopRepository>(MockBehavior.Strict);
            categoryRepositoryMock = new Mock<ICategoryRepository>(MockBehavior.Strict);
            warehouseRepositoryMock = new Mock<IWarehouseRepository>(MockBehavior.Strict);

            shopService = new ShopService(shopRepositoryMock.Object, 
                categoryRepositoryMock.Object,
                warehouseRepositoryMock.Object
            );
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // GetAllProductsAsync
        [Test]
        public async Task GetAllProductsAsync_ReturnsEmpty_WhenNoProducts()
        {
            var products = new List<Product>().BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = await shopService.GetAllProductsAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllProductsAsync_MapsAllPropertiesCorrectly()
        {
            var productId = Guid.NewGuid();
            var warehouse = new Warehouse { IsDeleted = false };
            var products = new List<Product>
            {
                new Product
                {
                    Id = productId,
                    Name = "Shirt",
                    Price = 29.99m,
                    ImageUrl = "img.jpg",
                    InStock = true,
                    Warehouse = warehouse
                }
            }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = (await shopService.GetAllProductsAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(productId));
            Assert.That(result[0].Name, Is.EqualTo("Shirt"));
            Assert.That(result[0].Price, Is.EqualTo(29.99m));
            Assert.That(result[0].ImageUrl, Is.EqualTo("img.jpg"));
            Assert.IsTrue(result[0].InStock);
        }

        [Test]
        public async Task GetAllProductsAsync_SetsDefaultImageUrl_WhenImageUrlIsNullOrEmpty()
        {
            var warehouse = new Warehouse { IsDeleted = false };
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Shirt",
                    Price = 29.99m,
                    ImageUrl = null,
                    InStock = true,
                    Warehouse = warehouse
                }
            }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = (await shopService.GetAllProductsAsync()).ToList();

            Assert.That(result[0].ImageUrl, Does.Contain("/images/"));
        }

        [Test]
        public async Task GetAllProductsAsync_ExcludesProductsFromDeletedWarehouses()
        {
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Shirt",
                    Price = 29.99m,
                    ImageUrl = "img.jpg",
                    InStock = true,
                    Warehouse = new Warehouse { IsDeleted = true }
                }
            }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = await shopService.GetAllProductsAsync();

            Assert.IsEmpty(result);
        }

        // AddProductAsync

        [Test]
        public async Task AddProductAsync_DoesNotAdd_WhenCategoryOrWarehouseOrGenderInvalid()
        {
            var input = new ProductFormInputModel
            {
                Name = "Shirt",
                Price = 10,
                Description = "desc",
                Size = "M",
                ImageUrl = "img.jpg",
                InStock = true,
                CategoryName = "Tops",
                Gender = "Invalid",
                WarehouseName = "Main"
            };

            categoryRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>())).ReturnsAsync((Category?)null);
            warehouseRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Warehouse, bool>>>())).ReturnsAsync((Warehouse?)null);

            await shopService.AddProductAsync(input);

            shopRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Never);
        }

        [Test]
        public async Task AddProductAsync_AddsProduct_WhenValid()
        {
            var input = new ProductFormInputModel
            {
                Name = "Shirt",
                Price = 10,
                Description = "desc",
                Size = "M",
                ImageUrl = "img.jpg",
                InStock = true,
                CategoryName = "Tops",
                Gender = "Men",
                WarehouseName = "Main"
            };
            var category = new Category { Id = 1, Name = "Tops" };
            var warehouse = new Warehouse { Id = Guid.NewGuid(), Name = "Main" };

            categoryRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>())).ReturnsAsync(category);
            warehouseRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Warehouse, bool>>>())).ReturnsAsync(warehouse);

            shopRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

            await shopService.AddProductAsync(input);

            shopRepositoryMock.Verify(r => r.AddAsync(It.Is<Product>(p =>
                p.Name == input.Name &&
                p.Price == input.Price &&
                p.Description == input.Description &&
                p.Size == input.Size &&
                p.ImageUrl == input.ImageUrl &&
                p.InStock == input.InStock &&
                p.CategoryId == category.Id &&
                p.WarehouseId == warehouse.Id &&
                p.GenderId == 1 // "Men" maps to 1
            )), Times.Once);
        }

        // GetProductDetailsByIdAsync
        [Test]
        public async Task GetProductDetailsByIdAsync_ReturnsNull_WhenNotFound()
        {
            var products = new List<Product>().BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = await shopService.GetProductDetailsByIdAsync(Guid.NewGuid());

            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductDetailsByIdAsync_MapsAllPropertiesCorrectly()
        {
            var productId = Guid.NewGuid();
            var category = new Category { Name = "Tops" };
            var gender = new Gender { Name = "Men" };
            var product = new Product
            {
                Id = productId,
                Name = "Shirt",
                Description = "desc",
                Price = 10,
                Size = "M",
                ImageUrl = "img.jpg",
                InStock = true,
                Category = category,
                Gender = gender
            };
            var products = new List<Product> { product }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = await shopService.GetProductDetailsByIdAsync(productId);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(productId));
            Assert.That(result.Name, Is.EqualTo("Shirt"));
            Assert.That(result.Description, Is.EqualTo("desc"));
            Assert.That(result.Price, Is.EqualTo(10));
            Assert.That(result.Size, Is.EqualTo("M"));
            Assert.That(result.ImageUrl, Is.EqualTo("img.jpg"));
            Assert.IsTrue(result.InStock);
            Assert.That(result.CategoryName, Is.EqualTo("Tops"));
            Assert.That(result.GenderName, Is.EqualTo("Men"));
        }

        [Test]
        public async Task GetProductDetailsByIdAsync_SetsDefaultImageUrl_WhenImageUrlIsNullOrEmpty()
        {
            var productId = Guid.NewGuid();
            var category = new Category { Name = "Tops" };
            var gender = new Gender { Name = "Men" };
            var product = new Product
            {
                Id = productId,
                Name = "Shirt",
                Description = "desc",
                Price = 10,
                Size = "M",
                ImageUrl = null,
                InStock = true,
                Category = category,
                Gender = gender
            };
            var products = new List<Product> { product }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = await shopService.GetProductDetailsByIdAsync(productId);

            Assert.That(result?.ImageUrl, Does.Contain("/images/"));
        }

        // GetProductForEditingAsync
        [Test]
        public async Task GetProductForEditingAsync_ReturnsNull_WhenProductIdIsNull()
        {
            var result = await shopService.GetProductForEditingAsync(null);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductForEditingAsync_ReturnsNull_WhenNotFound()
        {
            var products = new List<Product>().BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = await shopService.GetProductForEditingAsync(Guid.NewGuid());
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetProductForEditingAsync_MapsAllPropertiesCorrectly()
        {
            var productId = Guid.NewGuid();
            var category = new Category { Name = "Tops" };
            var gender = new Gender { Name = "Men" };
            var warehouse = new Warehouse { Name = "Main" };
            var product = new Product
            {
                Id = productId,
                Name = "Shirt",
                Description = "desc",
                Price = 10,
                Size = "M",
                ImageUrl = "img.jpg",
                InStock = true,
                Category = category,
                Gender = gender,
                Warehouse = warehouse
            };
            var products = new List<Product> { product }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = await shopService.GetProductForEditingAsync(productId);

            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(productId.ToString()));
            Assert.That(result.Name, Is.EqualTo("Shirt"));
            Assert.That(result.Description, Is.EqualTo("desc"));
            Assert.That(result.Price, Is.EqualTo(10));
            Assert.That(result.Size, Is.EqualTo("M"));
            Assert.That(result.ImageUrl, Is.EqualTo("img.jpg"));
            Assert.IsTrue(result.InStock);
            Assert.That(result.CategoryName, Is.EqualTo("Tops"));
            Assert.That(result.Gender, Is.EqualTo("Men"));
            Assert.That(result.WarehouseName, Is.EqualTo("Main"));
        }

        // EditProductAsync

        [Test]
        public async Task EditProductAsync_ReturnsFalse_WhenProductNotFound()
        {
            var input = new ProductFormInputModel { Id = Guid.NewGuid().ToString() };
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(new List<Product>().BuildMock());

            var result = await shopService.EditProductAsync(input);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditProductAsync_ReturnsFalse_WhenCategoryOrWarehouseOrGenderInvalid()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };
            var input = new ProductFormInputModel
            {
                Id = productId.ToString(),
                CategoryName = "Tops",
                Gender = "Invalid",
                WarehouseName = "Main"
            };

            var products = new List<Product> { product }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            categoryRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>())).ReturnsAsync((Category?)null);
            warehouseRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Warehouse, bool>>>())).ReturnsAsync((Warehouse?)null);

            var result = await shopService.EditProductAsync(input);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditProductAsync_UpdatesProduct_WhenValid()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };
            var input = new ProductFormInputModel
            {
                Id = productId.ToString(),
                Name = "Shirt",
                Price = 10,
                Description = "desc",
                Size = "M",
                ImageUrl = "img.jpg",
                InStock = true,
                CategoryName = "Tops",
                Gender = "Men",
                WarehouseName = "Main"
            };
            var category = new Category { Id = 1, Name = "Tops" };
            var warehouse = new Warehouse { Id = Guid.NewGuid(), Name = "Main" };

            var products = new List<Product> { product }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            categoryRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>())).ReturnsAsync(category);
            warehouseRepositoryMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Warehouse, bool>>>())).ReturnsAsync(warehouse);

            shopRepositoryMock.Setup(r => r.UpdateAsync(product)).ReturnsAsync(true);

            var result = await shopService.EditProductAsync(input);

            Assert.IsTrue(result);
            Assert.That(product.Name, Is.EqualTo("Shirt"));
            Assert.That(product.Price, Is.EqualTo(10));
            Assert.That(product.Description, Is.EqualTo("desc"));
            Assert.That(product.Size, Is.EqualTo("M"));
            Assert.That(product.ImageUrl, Is.EqualTo("img.jpg"));
            Assert.IsTrue(product.InStock);
            Assert.That(product.CategoryId, Is.EqualTo(category.Id));
            Assert.That(product.GenderId, Is.EqualTo(1));
            Assert.That(product.WarehouseId, Is.EqualTo(warehouse.Id));
        }

        // DeleteProductAsync

        [Test]
        public async Task DeleteProductAsync_ReturnsFalse_WhenProductNotFound()
        {
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(new List<Product>().BuildMock());

            var result = await shopService.DeleteProductAsync(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteProductAsync_DeletesProduct_WhenFound()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };
            var products = new List<Product> { product }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            shopRepositoryMock.Setup(r => r.HardDeleteAsync(product)).ReturnsAsync(true);

            var result = await shopService.DeleteProductAsync(productId.ToString());

            Assert.IsTrue(result);
        }

        // SoftDeleteProductAsync

        [Test]
        public async Task SoftDeleteProductAsync_ReturnsFalse_WhenProductNotFound()
        {
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(new List<Product>().BuildMock());

            var result = await shopService.SoftDeleteProductAsync(Guid.NewGuid().ToString());

            Assert.IsFalse(result);
        }

        [Test]
        public async Task SoftDeleteProductAsync_DeletesProduct_WhenFound()
        {
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId };
            var products = new List<Product> { product }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            shopRepositoryMock.Setup(r => r.DeleteAsync(product)).ReturnsAsync(true);

            var result = await shopService.SoftDeleteProductAsync(productId.ToString());

            Assert.IsTrue(result);
        }

        // GetProductsByGenderAsync
        [Test]
        public async Task GetProductsByGenderAsync_ReturnsEmpty_WhenNoProducts()
        {
            var products = new List<Product>().BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = await shopService.GetProductsByGenderAsync("Men");

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetProductsByGenderAsync_MapsAllPropertiesCorrectly()
        {
            var productId = Guid.NewGuid();
            var gender = new Gender { Name = "Men" };
            var products = new List<Product>
            {
                new Product
                {
                    Id = productId,
                    Name = "Shirt",
                    Price = 29.99m,
                    ImageUrl = "img.jpg",
                    InStock = true,
                    Gender = gender
                }
            }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = (await shopService.GetProductsByGenderAsync("Men")).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id,  Is.EqualTo(productId));
            Assert.That(result[0].Name, Is.EqualTo("Shirt"));
            Assert.That(result[0].Price, Is.EqualTo(29.99m));
            Assert.That(result[0].ImageUrl, Is.EqualTo("img.jpg"));
            Assert.IsTrue(result[0].InStock);
        }

        [Test]
        public async Task GetProductsByGenderAsync_SetsDefaultImageUrl_WhenImageUrlIsNullOrEmpty()
        {
            var gender = new Gender { Name = "Men" };
            var products = new List<Product>
            {
                new Product
                {
                    Id = Guid.NewGuid(),
                    Name = "Shirt",
                    Price = 29.99m,
                    ImageUrl = null,
                    InStock = true,
                    Gender = gender
                }
            }.BuildMock();
            shopRepositoryMock.Setup(r => r.GetAllAttached()).Returns(products);

            var result = (await shopService.GetProductsByGenderAsync("Men")).ToList();

            Assert.That(result[0].ImageUrl, Does.Contain("/images/"));
        }
    }
}