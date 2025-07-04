namespace ClothingBrandApp.Web.ViewModels.Product
{
    public class ProductDeleteInputModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageUrl { get; set; } 
    }
}
