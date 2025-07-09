using ClothingBrand.Services.Core.Interfaces;
using ClothingBrandApp.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

public class ShoppingCartController : BaseController
{
    private readonly IShoppingCartService shoppingCartService;

    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        this.shoppingCartService = shoppingCartService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        try
        {
            string? userId = this.GetUserId();
            if (userId == null)
            {
                return this.Forbid();
            }
            var userShoppingCartProducts = await this.shoppingCartService
                .GetAllProductsInShoppingCartAsync(userId);
            return View(userShoppingCartProducts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return RedirectToAction(nameof(Index), "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid? productId)
    {
        try
        {
            string? userId = this.GetUserId();
            if (userId == null)
            {
                // Not a valid case, added as defensive mechanism
                return this.Forbid();
            }

            bool result = await this.shoppingCartService
                    .AddProductToShoppingCartAsync(productId, userId);
            if (result == false)
            {
                // TODO: Add JS notifications
                return this.RedirectToAction(nameof(Index), "Shop");
            }

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            return this.RedirectToAction(nameof(Index), "Home");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid? productId)
    {
        try
        {
            string? userId = this.GetUserId();
            if (userId == null)
            {
                // Not a valid case, added as defensive mechanism
                return this.Forbid();
            }

            bool result = await this.shoppingCartService
                .DeleteProductFromShoppingCartAsync(productId, userId);
            if (result == false)
            {
                // TODO: Add JS notifications
                return this.RedirectToAction(nameof(Index), "Home");
            }

            return this.RedirectToAction(nameof(Index));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);

            return this.RedirectToAction(nameof(Index), "Home");
        }
    }
}