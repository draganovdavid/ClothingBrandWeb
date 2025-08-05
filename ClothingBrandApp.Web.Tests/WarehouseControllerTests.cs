using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.Controllers;
using ClothingBrandApp.Web.ViewModels.Warehouse;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClothingBrandApp.Web.Tests
{
    [TestFixture]
    public class WarehouseControllerTests
    {
        private Mock<IWarehouseService> warehouseServiceMock;
        private WarehouseController warehouseController;

        [SetUp]
        public void Setup()
        {
            this.warehouseServiceMock = new Mock<IWarehouseService>(MockBehavior.Strict);
            this.warehouseController = new WarehouseController(this.warehouseServiceMock.Object);
        }

        [Test]
        public void PassAlways()
        {
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            this.warehouseController?.Dispose();
        }

        [Test]
        public async Task Index_ReturnsViewWithDataFromWarehouseService()
        {
            var warehouseIndexViewModelCollection = new List<WarehouseIndexViewModel>
            {
                new WarehouseIndexViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Tarnovo Warehouse",
                    Location = "Veliko Tarnovo"
                },
                new WarehouseIndexViewModel
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Sofia Warehouse",
                    Location = "Sofia"
                }
            };

            warehouseServiceMock
                .Setup(ws => ws.GetAllWarehousesViewAsync())
                .ReturnsAsync(warehouseIndexViewModelCollection);

            var result = await warehouseController.Index();

            Assert.IsInstanceOf<ViewResult>(result);

            var viewResult = (ViewResult)result;
            Assert.IsInstanceOf<IEnumerable<WarehouseIndexViewModel>>(viewResult.ViewData.Model);

            var modelResult = (IEnumerable<WarehouseIndexViewModel>)viewResult.ViewData.Model!;
            Assert.That(modelResult.Count(), Is.EqualTo(warehouseIndexViewModelCollection.Count()));

            foreach (var viewModel in modelResult)
            {
                var seedDataViewModel = warehouseIndexViewModelCollection
                    .First(vm => vm.Id.ToLower() == viewModel.Id.ToLower());

                Assert.IsNotNull(seedDataViewModel);
                Assert.That(viewModel.Name, Is.EqualTo(seedDataViewModel.Name));
                Assert.That(viewModel.Location, Is.EqualTo(seedDataViewModel.Location));
            }
        }

        [Test]
        public async Task Index_RedirectsToHomeIndex_OnException()
        {
            warehouseServiceMock
                .Setup(ws => ws.GetAllWarehousesViewAsync())
                .ThrowsAsync(new Exception("Test exception"));

            var result = await warehouseController.Index();

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.EqualTo("Home"));
        }

        [Test]
        public async Task Stock_ReturnsView_WhenWarehouseStockViewModelIsNotNull()
        {
            var warehouseId = Guid.NewGuid().ToString();
            var stockViewModel = new WarehouseStockViewModel
            {
                WarehouseId = warehouseId,
                WarehouseName = "Test Warehouse",
                WarehouseManager = "Manager",
                WarehouseProducts = new List<WarehouseStockProductViewModel>()
            };

            warehouseServiceMock
                .Setup(ws => ws.GetWarehouseProductsAsync(warehouseId))
                .ReturnsAsync(stockViewModel);

            var result = await warehouseController.Stock(warehouseId);

            Assert.IsInstanceOf<ViewResult>(result);
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOf<WarehouseStockViewModel>(viewResult.Model);
            Assert.That(((WarehouseStockViewModel)viewResult.Model!).WarehouseId, Is.EqualTo(warehouseId));
        }

        [Test]
        public async Task Stock_RedirectsToIndex_WhenWarehouseStockViewModelIsNull()
        {
            warehouseServiceMock
                .Setup(ws => ws.GetWarehouseProductsAsync(It.IsAny<string>()))
                .ReturnsAsync((WarehouseStockViewModel?)null);

            var result = await warehouseController.Stock("nonexistent-id");

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.Null);
        }

        [Test]
        public async Task Stock_RedirectsToIndex_OnException()
        {
            warehouseServiceMock
                .Setup(ws => ws.GetWarehouseProductsAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var result = await warehouseController.Stock("any-id");

            Assert.IsInstanceOf<RedirectToActionResult>(result);
            var redirectResult = (RedirectToActionResult)result;
            Assert.That(redirectResult.ActionName, Is.EqualTo("Index"));
            Assert.That(redirectResult.ControllerName, Is.Null);
        }
    }
}