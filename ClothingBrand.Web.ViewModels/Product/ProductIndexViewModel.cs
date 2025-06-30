using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingBrandApp.Web.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        // Used for the favorite heart icon in the view
        public bool IsFavorite { get; set; }
    }
}
