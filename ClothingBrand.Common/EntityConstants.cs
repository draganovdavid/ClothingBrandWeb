namespace ClothingBrand.Data.Common
{
    public static class EntityConstants
    {
        public static class Product
        {
            /// <summary>
            /// Product Title should be at least 2 characters and up to 100 characters.
            /// </summary>
            public const int NameMinLength = 2;

            /// <summary>
            /// Product Title should be able to store text with length up to 100 characters.
            /// </summary>
            public const int NameMaxLength = 100;

            /// <summary>
            /// Category must be at least 3 characters.
            /// </summary>
            public const int CategoryMinLength = 3;

            /// <summary>
            /// Product Category should be able to store text with length up to 50 characters.
            /// </summary>
            public const int CategoryMaxLength = 50;

            /// <summary>
            /// Gender must be at least 4 characters.
            /// </summary>
            public const int GenderMinLength = 4;

            /// <summary>
            /// Gender must be max 6 characters.
            /// </summary>
            public const int GenderMaxLength = 6;

            /// <summary>
            /// Product Size must be at least 1 characters.
            /// </summary>
            public const int SizeMinLength = 1;

            /// <summary>
            /// Product Size should be able to store text with length up to 100 characters.
            /// </summary>
            public const int SizeMaxLength = 10;

            /// <summary>
            /// Product Description must be at least 10 characters.
            /// </summary>
            public const int DescriptionMinLength = 10;

            /// <summary>
            /// Product Description should be able to store text with length up to 1000 characters.
            /// </summary>
            public const int DescriptionMaxLength = 1000;

            /// <summary>
            /// Maximum allowed length for image URL.
            /// </summary>
            public const int ImageUrlMaxLength = 2048;
        }

        public static class Category
        {
            /// <summary>
            /// Category must be at least 3 characters.
            /// </summary>
            public const int CategoryMinLength = 3;

            /// <summary>
            /// Product Category should be able to store text with length up to 50 characters.
            /// </summary>
            public const int CategoryMaxLength = 50;
        }
    }
}