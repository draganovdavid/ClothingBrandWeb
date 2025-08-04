namespace ClothingBrandApp.Web.ViewModels.Warehouse
{
    public class WarehouseStockProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Gender { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Price { get; set; }
        public bool InStock { get; set; }
        public bool IsDeleted { get; set; }
    }
}