using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core;
using ClothingBrand.Services.Core.Interfaces;
using MockQueryable;
using Moq;
using System.Linq.Expressions;

namespace ClothingBrandApp.Services.Tests
{
    [TestFixture]
    public class ManagerServiceTests
    {
        private Mock<IManagerRepository> managerRepositoryMock;
        private IManagerService managerService;

        [SetUp]
        public void Setup()
        {
            managerRepositoryMock = new Mock<IManagerRepository>(MockBehavior.Strict);
            managerService = new ManagerService(managerRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        private static readonly string?[] NullOrWhiteSpaceUserIds = new string?[] { null, "", "   " };
        [TestCaseSource(nameof(NullOrWhiteSpaceUserIds))]
        [TestCase("")]
        [TestCase("   ")]
        public async Task GetIdByUserIdAsync_ReturnsNull_ForNullOrWhitespaceUserId(string userId)
        {
            var result = await managerService.GetIdByUserIdAsync(userId);
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetIdByUserIdAsync_ReturnsNull_WhenManagerNotFound()
        {
            managerRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Manager, bool>>>()))
                .ReturnsAsync((Manager?)null);

            var result = await managerService.GetIdByUserIdAsync("user123");
            Assert.IsNull(result);
        }

        [TestCase("user123")]
        [TestCase("UserABC")]
        public async Task GetIdByUserIdAsync_ReturnsId_WhenManagerExists_CaseInsensitive(string queryUserId)
        {
            var expectedId = Guid.NewGuid();
            var manager = new Manager { Id = expectedId, UserId = "UserABC" };

            managerRepositoryMock
                .Setup(r => r.FirstOrDefaultAsync(It.IsAny<Expression<Func<Manager, bool>>>()))
                .ReturnsAsync(manager);

            var result = await managerService.GetIdByUserIdAsync(queryUserId);
            Assert.That(result, Is.EqualTo(expectedId));
        }

        [TestCaseSource(nameof(NullOrWhiteSpaceUserIds))]
        [TestCase("")]
        [TestCase("   ")]
        public async Task ExistsByIdAsync_ReturnsFalse_ForNullOrWhitespaceId(string id)
        {
            var exists = await managerService.ExistsByIdAsync(id);
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task ExistsByIdAsync_ReturnsFalse_WhenManagerNotFound()
        {
            var emptyList = new List<Manager>().BuildMock();
            managerRepositoryMock.Setup(r => r.GetAllAttached()).Returns(emptyList);

            var exists = await managerService.ExistsByIdAsync(Guid.NewGuid().ToString());
            Assert.IsFalse(exists);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task ExistsByIdAsync_ReturnsTrue_WhenManagerExists_CaseInsensitive(bool useUpperCase)
        {
            var managerId = Guid.NewGuid();
            var list = new List<Manager> { new Manager { Id = managerId } }.BuildMock();
            managerRepositoryMock.Setup(r => r.GetAllAttached()).Returns(list);

            var input = useUpperCase
                ? managerId.ToString().ToUpperInvariant()
                : managerId.ToString();

            var exists = await managerService.ExistsByIdAsync(input);
            Assert.IsTrue(exists);
        }

        [TestCaseSource(nameof(NullOrWhiteSpaceUserIds))]
        [TestCase("")]
        [TestCase("   ")]
        public async Task ExistsByUserIdAsync_ReturnsFalse_ForNullOrWhitespaceUserId(string userId)
        {
            var exists = await managerService.ExistsByUserIdAsync(userId);
            Assert.IsFalse(exists);
        }

        [Test]
        public async Task ExistsByUserIdAsync_ReturnsFalse_WhenManagerNotFound()
        {
            var emptyList = new List<Manager>().BuildMock();
            managerRepositoryMock.Setup(r => r.GetAllAttached()).Returns(emptyList);

            var exists = await managerService.ExistsByUserIdAsync("nonexistent");
            Assert.IsFalse(exists);
        }
    }
}