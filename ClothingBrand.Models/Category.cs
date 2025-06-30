namespace ClothingBrand.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; //Collection: Men, Women, Kids

        public virtual ICollection<Product> Products { get; set; } 
            = new HashSet<Product>();
    }
}
