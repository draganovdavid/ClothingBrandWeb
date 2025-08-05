using ClothingBrand.Data.Models;
using ClothingBrand.Data.Repository.Interfaces;
using ClothingBrand.Services.Core;
using ClothingBrand.Services.Core.Interfaces;
using MockQueryable;
using Moq;

namespace ClothingBrandApp.Services.Tests
{
    [TestFixture]
    public class CategoryServiceTests
    {
        private Mock<ICategoryRepository> categoryRepositoryMock;
        private ICategoryService categoryService;

        [SetUp]
        public void Setup()
        {
            categoryRepositoryMock = new Mock<ICategoryRepository>(MockBehavior.Strict);
            categoryService = new CategoryService(categoryRepositoryMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            // Test that will always pass to show that the SetUp is working
            Assert.Pass();
        }

        [Test]
        public async Task GetAllCategoriesDropDownAsync_ReturnsEmpty_WhenNoCategories()
        {
            var categories = new List<Category>().BuildMock();
            categoryRepositoryMock.Setup(r => r.GetAllAttached()).Returns(categories);

            var result = await categoryService.GetAllCategoriesDropDownAsync();

            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllCategoriesDropDownAsync_ReturnsAllCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Tops" },
                new Category { Id = 2, Name = "Bottoms" }
            }.BuildMock();
            categoryRepositoryMock.Setup(r => r.GetAllAttached()).Returns(categories);

            var result = (await categoryService.GetAllCategoriesDropDownAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Tops"));
            Assert.That(result[1].Name, Is.EqualTo("Bottoms"));
        }

        [Test]
        public async Task GetAllCategoriesDropDownAsync_HandlesCategoryWithEmptyName()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "" }
            }.BuildMock();
            categoryRepositoryMock.Setup(r => r.GetAllAttached()).Returns(categories);

            var result = (await categoryService.GetAllCategoriesDropDownAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo(""));
        }

        [Test]
        public async Task GetAllCategoriesDropDownAsync_HandlesCategoryWithNullName()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = string.Empty }
            }.BuildMock();
            categoryRepositoryMock.Setup(r => r.GetAllAttached()).Returns(categories);

            var result = (await categoryService.GetAllCategoriesDropDownAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task GetAllCategoriesDropDownAsync_ReturnsDistinctNames_WhenDuplicateCategories()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Tops" },
                new Category { Id = 2, Name = "Tops" }
            }.BuildMock();
            categoryRepositoryMock.Setup(r => r.GetAllAttached()).Returns(categories);

            var result = (await categoryService.GetAllCategoriesDropDownAsync()).ToList();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Name, Is.EqualTo("Tops"));
            Assert.That(result[1].Name, Is.EqualTo("Tops"));
        }
    }
}