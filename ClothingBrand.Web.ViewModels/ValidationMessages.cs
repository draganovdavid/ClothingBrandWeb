namespace ClothingBrandApp.Web.ViewModels
{
    public static class ValidationMessages
    {
        public static class Product
        {
            // Error messages for Product entity
            public const string NameRequiredMessage = "Product name is required.";
            public const string NameMinLengthMessage = "Product name must be at least 2 characters.";
            public const string NameMaxLengthMessage = "Product name cannot exceed 100 characters.";

            public const string CategoryRequiredMessage = "Category is required.";
            public const string CategoryMinLengthMessage = "Category must be at least 3 characters.";
            public const string CategoryMaxLengthMessage = "Category cannot exceed 50 characters.";

            public const string GenderRequiredMessage = "Gender is required.";
            public const string GenderMinLengthMessage = "Gender must be at least 3 characters.";
            public const string GenderMaxLengthMessage = "Gender cannot exceed 10 characters.";

            public const string SizeRequiredMessage = "Size is required.";
            public const string SizeMinLengthMessage = "Size must be at least 1 character.";
            public const string SizeMaxLengthMessage = "Size cannot exceed 10 characters.";

            public const string DescriptionRequiredMessage = "Description is required.";
            public const string DescriptionMinLengthMessage = "Description must be at least 10 characters.";
            public const string DescriptionMaxLengthMessage = "Description cannot exceed 1000 characters.";

            public const string PriceRequiredMessage = "Price is required.";
            public const string PriceRangeMessage = "Price must be a positive value.";

            public const string ImageUrlMaxLengthMessage = "Image URL cannot exceed 2048 characters.";

            public const string InStockRequiredMessage = "Stock status is required.";

            public const string ServiceCreateErrorMessage =
                "A fatal error occurred while adding your product! Please try again later!";
        }

        public static class Warehouse
        {
            public const string WarehouseRequiredMessage = "Warehouse name is required.";
            public const string WarehouseMinLengthMessage = "Warehouse name must be at least 2 characters.";
            public const string WarehouseMaxLengthMessage = "Warehouse name cannot exceed 100 characters.";

            public const string WarehouseLocationRequiredMessage = "Warehouse location is required.";
            public const string WarehouseLocationMinLengthMessage = "Warehouse location must be at least 2 characters.";
            public const string WarehouseLocationMaxLengthMessage = "Warehouse location cannot exceed 50 characters.";
        }
    }
}
