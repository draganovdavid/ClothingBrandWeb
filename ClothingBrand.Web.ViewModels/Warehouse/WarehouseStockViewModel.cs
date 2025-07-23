namespace ClothingBrandApp.Web.ViewModels.Warehouse
{
    public class WarehouseStockViewModel
    {
        public string WarehouseId { get; set; } = null!;

        public string WarehouseName { get; set; } = null!;

        public IEnumerable<WarehouseStockViewModel> WarehouseProducts { get; set; } = null!;
    }
}
