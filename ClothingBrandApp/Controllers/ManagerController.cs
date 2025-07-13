using Microsoft.AspNetCore.Mvc;

namespace ClothingBrandApp.Web.Controllers
{
    public class ManagerController : BaseController
    {
        public IActionResult Index()
        {
            return this.Ok("I am manager!!!");
        }
    }
}
