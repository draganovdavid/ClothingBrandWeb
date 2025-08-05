using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core;
using ClothingBrand.Services.Core.Interfaces;
using MockQueryable;
using Moq;

namespace ClothingBrandApp.Services.Tests
{
    [TestFixture]
    public class WarehouseServiceTests
    {
        private Mock<IWarehouseRepository> warehouseRepositoryMock;
        private IWarehouseService warehouseService;

        [SetUp]
        public void Setup()
        {
            warehouseRepositoryMock = new Mock<IWarehouseRepository>(MockBehavior.Strict);
            warehouseService = new WarehouseService(warehouseRepositoryMock.Object);
        }


        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // 1. GetAllWarehousesViewAsync
        [Test]
        public async Task GetAllWarehousesViewAsync_ReturnsEmpty_WhenNoWarehouses()
        {
            var warehouses = new List<Warehouse>().BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = await warehouseService.GetAllWarehousesViewAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllWarehousesViewAsync_MapsAllPropertiesCorrectly()
        {
            var warehouseId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = warehouseId,
                    Name = "Warehouse 1",
                    Location = "Sofia",
                    ManagerId = managerId
                }
            }.BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = (await warehouseService.GetAllWarehousesViewAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(warehouseId.ToString()));
            Assert.That(result[0].Name, Is.EqualTo("Warehouse 1"));
            Assert.That(result[0].Location, Is.EqualTo("Sofia"));
            Assert.That(result[0].ManagerId, Is.EqualTo(managerId.ToString()));
        }

        [Test]
        public async Task GetAllWarehousesViewAsync_HandlesNullManagerId()
        {
            var warehouseId = Guid.NewGuid();
            var warehouses = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = warehouseId,
                    Name = "Warehouse 1",
                    Location = "Sofia",
                    ManagerId = null
                }
            }.BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = (await warehouseService.GetAllWarehousesViewAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].ManagerId, Is.EqualTo(string.Empty));
        }

        // 2. GetWarehouseProductsAsync
        [Test]
        public async Task GetWarehouseProductsAsync_ReturnsNull_WhenIdIsNull()
        {
            var result = await warehouseService.GetWarehouseProductsAsync(null);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetWarehouseProductsAsync_ReturnsNull_WhenIdIsWhitespace()
        {
            var result = await warehouseService.GetWarehouseProductsAsync("   ");
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetWarehouseProductsAsync_ReturnsNull_WhenWarehouseNotFound()
        {
            var warehouses = new List<Warehouse>().BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = await warehouseService.GetWarehouseProductsAsync(Guid.NewGuid().ToString());
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetWarehouseProductsAsync_ReturnsViewModel_WhenWarehouseExists_WithManagerAndProducts()
        {
            var warehouseId = Guid.NewGuid();
            var managerId = Guid.NewGuid();
            var user = new ApplicationUser { UserName = "manageruser" };
            var manager = new Manager { Id = managerId, User = user };
            var gender = new Gender { Id = 1, Name = "Men" };
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "T-Shirt",
                Description = "A nice t-shirt",
                Price = 19.99m,
                Size = "M",
                ImageUrl = "img.jpg",
                InStock = true,
                Gender = gender,
                GenderId = gender.Id,
                IsDeleted = false
            };
            var warehouse = new Warehouse
            {
                Id = warehouseId,
                Name = "Main Warehouse",
                Location = "Sofia",
                Manager = manager,
                ManagerId = managerId,
                WarehouseProducts = new List<Product> { product }
            };
            var warehouses = new List<Warehouse> { warehouse }.BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = await warehouseService.GetWarehouseProductsAsync(warehouseId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.WarehouseId, Is.EqualTo(warehouseId.ToString()));
            Assert.That(result.WarehouseName, Is.EqualTo("Main Warehouse - Sofia"));
            Assert.That(result.WarehouseManager, Is.EqualTo("manageruser"));
            Assert.That(result.WarehouseProducts.Count(), Is.EqualTo(1));
            var prod = result.WarehouseProducts.First();
            Assert.That(prod.Id, Is.EqualTo(product.Id));
            Assert.That(prod.Name, Is.EqualTo("T-Shirt"));
            Assert.That(prod.Price, Is.EqualTo(19.99m));
            Assert.That(prod.ImageUrl, Is.EqualTo("img.jpg"));
            Assert.That(prod.Gender, Is.EqualTo("Men"));
            Assert.IsTrue(prod.InStock);
            Assert.IsFalse(prod.IsDeleted);
        }

        [Test]
        public async Task GetWarehouseProductsAsync_ReturnsViewModel_WithUnassignedManager()
        {
            var warehouseId = Guid.NewGuid();
            var gender = new Gender { Id = 2, Name = "Male" };
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = "Jeans",
                Description = "Blue jeans",
                Price = 49.99m,
                Size = "L",
                ImageUrl = "jeans.jpg",
                InStock = false,
                Gender = gender,
                GenderId = gender.Id,
                IsDeleted = true
            };
            var warehouse = new Warehouse
            {
                Id = warehouseId,
                Name = "Secondary Warehouse",
                Location = "Plovdiv",
                Manager = null,
                ManagerId = null,
                WarehouseProducts = new List<Product> { product }
            };
            var warehouses = new List<Warehouse> { warehouse }.BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = await warehouseService.GetWarehouseProductsAsync(warehouseId.ToString());

            Assert.IsNotNull(result);
            Assert.That(result.WarehouseManager, Is.EqualTo("Unassigned"));
        }

        [Test]
        public async Task GetWarehouseProductsAsync_HandlesCaseInsensitiveId()
        {
            var warehouseId = Guid.NewGuid();
            var warehouse = new Warehouse
            {
                Id = warehouseId,
                Name = "Case Warehouse",
                Location = "Varna",
                Manager = null,
                ManagerId = null,
                WarehouseProducts = new List<Product>()
            };
            var warehouses = new List<Warehouse> { warehouse }.BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = await warehouseService.GetWarehouseProductsAsync(warehouseId.ToString().ToUpperInvariant());

            Assert.IsNotNull(result);
            Assert.That(result.WarehouseId, Is.EqualTo(warehouseId.ToString()));
        }

        [Test]
        public async Task GetWarehouseProductsAsync_ReturnsEmptyProducts_WhenNoProducts()
        {
            var warehouseId = Guid.NewGuid();
            var warehouse = new Warehouse
            {
                Id = warehouseId,
                Name = "Empty Warehouse",
                Location = "Burgas",
                Manager = null,
                ManagerId = null,
                WarehouseProducts = new List<Product>()
            };
            var warehouses = new List<Warehouse> { warehouse }.BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = await warehouseService.GetWarehouseProductsAsync(warehouseId.ToString());

            Assert.IsNotNull(result);
            Assert.IsEmpty(result.WarehouseProducts);
        }

        // 3. GetAllWarehousesDropDownAsync
        [Test]
        public async Task GetAllWarehousesDropDownAsync_ReturnsEmpty_WhenNoWarehouses()
        {
            var warehouses = new List<Warehouse>().BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = await warehouseService.GetAllWarehousesDropDownAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllWarehousesDropDownAsync_ReturnsNamesCorrectly()
        {
            var warehouses = new List<Warehouse>
            {
                new Warehouse { Name = "Warehouse 1" },
                new Warehouse { Name = "Warehouse 2" }
            }.BuildMock();
            warehouseRepositoryMock.Setup(r => r.GetAllAttached()).Returns(warehouses);

            var result = (await warehouseService.GetAllWarehousesDropDownAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Warehouse 1"));
            Assert.That(result[1].Name, Is.EqualTo("Warehouse 2"));
        }
    }
}