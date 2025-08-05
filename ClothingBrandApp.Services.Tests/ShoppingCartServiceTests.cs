using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core.Interfaces;
using MockQueryable;
using Moq;

namespace ClothingBrandApp.Services.Tests
{
    [TestFixture]
    public class ShoppingCartServiceTests
    {
        private Mock<IShoppingCartRepository> shoppingCartRepositoryMock;
        private IShoppingCartService shoppingCartService;

        [SetUp]
        public void Setup()
        {
            shoppingCartRepositoryMock = new Mock<IShoppingCartRepository>(MockBehavior.Strict);
            shoppingCartService = new ShoppingCartService(shoppingCartRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        // GetAllProductsInShoppingCartAsync
        [Test]
        public async Task GetAllProductsInShoppingCartAsync_ReturnsEmpty_WhenNoProducts()
        {
            var userId = "user1";
            var cartItems = new List<ApplicationUserShoppingCart>().BuildMock();
            shoppingCartRepositoryMock.Setup(r => r.GetAllAttached()).Returns(cartItems);

            var result = await shoppingCartService.GetAllProductsInShoppingCartAsync(userId);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllProductsInShoppingCartAsync_ReturnsMappedProducts()
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
            var cartItems = new List<ApplicationUserShoppingCart>
            {
                new ApplicationUserShoppingCart
                {
                    ApplicationUserId = userId,
                    ProductId = productId,
                    Product = product
                }
            }.BuildMock();
            shoppingCartRepositoryMock.Setup(r => r.GetAllAttached()).Returns(cartItems);

            var result = (await shoppingCartService.GetAllProductsInShoppingCartAsync(userId)).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Id, Is.EqualTo(productId));
            Assert.That(result[0].Name, Is.EqualTo("Shirt"));
            Assert.That(result[0].Price, Is.EqualTo(29.99m));
            Assert.That(result[0].ImageUrl, Is.EqualTo("img.jpg"));
        }

        [Test]
        public async Task GetAllProductsInShoppingCartAsync_ReturnsEmpty_WhenUserIdHasNoProducts()
        {
            var userId = "user1";
            var cartItems = new List<ApplicationUserShoppingCart>
            {
                new ApplicationUserShoppingCart
                {
                    ApplicationUserId = "otheruser",
                    ProductId = Guid.NewGuid(),
                    Product = new Product { Id = Guid.NewGuid(), Name = "Other", Price = 10 }
                }
            }.BuildMock();
            shoppingCartRepositoryMock.Setup(r => r.GetAllAttached()).Returns(cartItems);

            var result = await shoppingCartService.GetAllProductsInShoppingCartAsync(userId);

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        // AddProductToShoppingCartAsync
        [Test]
        public async Task AddProductToShoppingCartAsync_ReturnsFalse_WhenProductIdIsNull()
        {
            var result = await shoppingCartService.AddProductToShoppingCartAsync(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task AddProductToShoppingCartAsync_UpdatesIfAlreadyExists()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();
            var entry = new ApplicationUserShoppingCart
            {
                ApplicationUserId = userId,
                ProductId = productId,
                IsDeleted = true
            };

            shoppingCartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<ApplicationUserShoppingCart> { entry }.BuildMock());

            shoppingCartRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserShoppingCart, bool>>>()))
                .ReturnsAsync(entry);

            shoppingCartRepositoryMock
                .Setup(r => r.UpdateAsync(entry))
                .ReturnsAsync(true);

            var result = await shoppingCartService.AddProductToShoppingCartAsync(productId, userId);

            Assert.IsTrue(result);
            Assert.IsFalse(entry.IsDeleted);
        }

        [Test]
        public async Task AddProductToShoppingCartAsync_AddsIfNotExists()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();

            shoppingCartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<ApplicationUserShoppingCart>().BuildMock());

            shoppingCartRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserShoppingCart, bool>>>()))
                .ReturnsAsync((ApplicationUserShoppingCart?)null);

            shoppingCartRepositoryMock
                .Setup(r => r.AddAsync(It.Is<ApplicationUserShoppingCart>(aup => aup.ApplicationUserId == userId && aup.ProductId == productId)))
                .Returns(Task.CompletedTask);

            var result = await shoppingCartService.AddProductToShoppingCartAsync(productId, userId);

            Assert.IsTrue(result);
        }

        // DeleteProductFromShoppingCartAsync
        [Test]
        public async Task DeleteProductFromShoppingCartAsync_ReturnsFalse_WhenProductIdIsNull()
        {
            var result = await shoppingCartService.DeleteProductFromShoppingCartAsync(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteProductFromShoppingCartAsync_ReturnsFalse_WhenEntryNotFound()
        {
            shoppingCartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<ApplicationUserShoppingCart>().BuildMock());

            shoppingCartRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserShoppingCart, bool>>>()))
                .ReturnsAsync((ApplicationUserShoppingCart?)null);

            var result = await shoppingCartService.DeleteProductFromShoppingCartAsync(Guid.NewGuid(), "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task DeleteProductFromShoppingCartAsync_DeletesIfEntryFound()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();
            var entry = new ApplicationUserShoppingCart
            {
                ApplicationUserId = userId,
                ProductId = productId,
                IsDeleted = false
            };

            shoppingCartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(new List<ApplicationUserShoppingCart> { entry }.BuildMock());

            shoppingCartRepositoryMock
                .Setup(r => r.SingleOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<ApplicationUserShoppingCart, bool>>>()))
                .ReturnsAsync(entry);

            shoppingCartRepositoryMock
                .Setup(r => r.UpdateAsync(entry))
                .ReturnsAsync(true);

            shoppingCartRepositoryMock
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await shoppingCartService.DeleteProductFromShoppingCartAsync(productId, userId);

            Assert.IsTrue(result);
            Assert.IsTrue(entry.IsDeleted);
        }

        // IsProductAddedToShoppingCart
        [Test]
        public async Task IsProductAddedToShoppingCart_ReturnsFalse_WhenProductIdIsNull()
        {
            var result = await shoppingCartService.IsProductAddedToShoppingCart(null, "user1");
            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsProductAddedToShoppingCart_ReturnsFalse_WhenNotInCart()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();
            var cartItems = new List<ApplicationUserShoppingCart>().BuildMock();
            shoppingCartRepositoryMock.Setup(r => r.GetAllAttached()).Returns(cartItems);

            shoppingCartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(cartItems);

            var result = await shoppingCartService.IsProductAddedToShoppingCart(productId, userId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task IsProductAddedToShoppingCart_ReturnsTrue_WhenInCart()
        {
            var userId = "user1";
            var productId = Guid.NewGuid();
            var cartItems = new List<ApplicationUserShoppingCart>
            {
                new ApplicationUserShoppingCart
                {
                    ApplicationUserId = userId,
                    ProductId = productId
                }
            }.BuildMock();

            shoppingCartRepositoryMock
                .Setup(r => r.GetAllAttached())
                .Returns(cartItems);

            var result = await shoppingCartService.IsProductAddedToShoppingCart(productId, userId);

            Assert.IsTrue(result);
        }
    }
}