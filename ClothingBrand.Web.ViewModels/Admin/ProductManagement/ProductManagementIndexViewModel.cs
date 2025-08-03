namespace ClothingBrandApp.Web.ViewModels.Admin.ProductManagement
{
    public class ProductManagementIndexViewModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public string Size { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public string Category { get; set; } = null!;

        public bool InStock { get; set; }

        public bool IsDeleted { get; set; }
    }
}
