using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core;
using ClothingBrand.Services.Core.Interfaces;
using MockQueryable;
using Moq;

namespace ClothingBrandApp.Services.Tests
{
    [TestFixture]
    public class FavoriteServiceTests
    {
        private Mock<IFavoriteRepository> favoriteRepositoryMock;
        private IFavoriteService favoriteService;

        [SetUp]
        public void Setup()
        {
            favoriteRepositoryMock = new Mock<IFavoriteRepository>(MockBehavior.Strict);
            favoriteService = new FavoriteService(favoriteRepositoryMock.Object);
        }


        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // GetUserFavoriteProductsAsync

        [Test]
        public async Task GetUserFavoriteProductsAsync_ReturnsEmpty_WhenNoFavorites()
        {
            var userId = "user1";
            var favorites = new List<ApplicationUserProduct>().BuildMock();
            favoriteRepositoryMock.Setup(r => r.GetAllAttached()).Returns(favorites);

            var result = await favoriteService.GetUserFavoriteProductsAsync(userId);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetUserFavoriteProductsAsync_ReturnsMappedProducts()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Shirt",
                Price = 29.99m,
                ImageUrl = "img.jpg"
            };
            var favorites = new List<ApplicationUserProduct>
            {
                new ApplicationUserProduct
                {
                    ApplicationUserId = userId,
                    ProductId = productId,
                    Product = product
                }
            }.BuildMock();
            favoriteRepositoryMock.Setup(r => r.GetAllAttached()).Returns(favorites);

            var result = await favoriteService.GetUserFavoriteProductsAsync(userId);

            Assert.IsNotNull(result);
            var resultList = result?.ToList();

            Assert.That(resultList?.Count, Is.EqualTo(1));
            Assert.That(resultList?[0].Id, Is.EqualTo(productId));
            Assert.That(resultList?[0].Name, Is.EqualTo("Shirt"));
            Assert.That(resultList?[0].Price, Is.EqualTo(29.99m));
            Assert.That(resultList?[0].ImageUrl, Is.EqualTo("img.jpg"));
        }

        [Test]
        public async Task GetUserFavoriteProductsAsync_ReturnsEmpty_WhenUserIdHasNoFavorites()
        {
            var userId = "user1";
            var favorites = new List<ApplicationUserProduct>
            {
                new ApplicationUserProduct
                {
                    ApplicationUserId = "otheruser",
                    ProductId = Guid.NewGuid(),
                    Product = new Product { Id = Guid.NewGuid(), Name = "Other", Price = 10 }
                }
            }.BuildMock();
            favoriteRepositoryMock.Setup(r => r.GetAllAttached()).Returns(favorites);

            var result = await favoriteService.GetUserFavoriteProductsAsync(userId);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        // AddProductToUserFavoritesAsync
        [Test]
        public async Task AddProductToUserFavoritesAsync_ReturnsFalse_WhenProductIdIsNull()
        {
            var result = await favoriteService.AddProductToUserFavoritesAsync(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddProductToUserFavoritesAsync_UpdatesIfAlreadyExists()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();
            var entry = new ApplicationUserProduct
            {
                ApplicationUserId = userId,
                ProductId = productId,
                IsDeleted = true
            };

            favoriteRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<ApplicationUserProduct> { entry }.BuildMock());

            favoriteRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserProduct, bool>>>()))
                .ReturnsAsync(entry);

            favoriteRepositoryMock
                .Setup(r => r.UpdateAsync(entry))
                .ReturnsAsync(true);

            var result = await favoriteService.AddProductToUserFavoritesAsync(productId, userId);

            Assert.IsTrue(result);
            Assert.IsFalse(entry.IsDeleted);
        }

        [Test]
        public async Task AddProductToUserFavoritesAsync_AddsIfNotExists()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();

            favoriteRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<ApplicationUserProduct>().BuildMock());

            favoriteRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserProduct, bool>>>()))
                .ReturnsAsync((ApplicationUserProduct?)null);

            favoriteRepositoryMock
                .Setup(r => r.AddAsync(It.Is<ApplicationUserProduct>(aup => aup.ApplicationUserId == userId && aup.ProductId == productId)))
                .Returns(Task.CompletedTask);

            var result = await favoriteService.AddProductToUserFavoritesAsync(productId, userId);

            Assert.IsTrue(result);
        }

        // DeleteProductFromUserFavoritesAsync
        [Test]
        public async Task DeleteProductFromUserFavoritesAsync_ReturnsFalse_WhenProductIdIsNull()
        {
            var result = await favoriteService.DeleteProductFromUserFavoritesAsync(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteProductFromUserFavoritesAsync_ReturnsFalse_WhenUserIdIsNull()
        {
            var result = await favoriteService.DeleteProductFromUserFavoritesAsync(Guid.NewGuid(), null);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteProductFromUserFavoritesAsync_ReturnsFalse_WhenEntryNotFound()
        {
            favoriteRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserProduct, bool>>>()))
                .ReturnsAsync((ApplicationUserProduct?)null);

            var result = await favoriteService.DeleteProductFromUserFavoritesAsync(Guid.NewGuid(), "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteProductFromUserFavoritesAsync_DeletesIfEntryFound()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();
            var entry = new ApplicationUserProduct
            {
                ApplicationUserId = userId,
                ProductId = productId,
                IsDeleted = false
            };

            favoriteRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserProduct, bool>>>()))
                .ReturnsAsync(entry);

            favoriteRepositoryMock
                .Setup(r => r.DeleteAsync(entry))
                .ReturnsAsync(true);

            var result = await favoriteService.DeleteProductFromUserFavoritesAsync(productId, userId);

            Assert.IsTrue(result);
            Assert.IsTrue(entry.IsDeleted);
        }

        // IsProductAddedToFavorites
        [Test]
        public async Task IsProductAddedToFavorites_ReturnsFalse_WhenProductIdIsNull()
        {
            var result = await favoriteService.IsProductAddedToFavorites(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsProductAddedToFavorites_ReturnsFalse_WhenUserIdIsNull()
        {
            var result = await favoriteService.IsProductAddedToFavorites(Guid.NewGuid(), null);
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsProductAddedToFavorites_ReturnsFalse_WhenEntryNotFound()
        {
            favoriteRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserProduct, bool>>>()))
                .ReturnsAsync((ApplicationUserProduct?)null);

            var result = await favoriteService.IsProductAddedToFavorites(Guid.NewGuid(), "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsProductAddedToFavorites_ReturnsTrue_WhenEntryFound()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();
            var entry = new ApplicationUserProduct
            {
                ApplicationUserId = userId,
                ProductId = productId
            };

            favoriteRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserProduct, bool>>>()))
                .ReturnsAsync(entry);

            var result = await favoriteService.IsProductAddedToFavorites(productId, userId);

            Assert.IsTrue(result);
        }
    }
}