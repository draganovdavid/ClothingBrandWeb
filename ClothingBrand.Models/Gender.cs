namespace ClothingBrand.Data.Models
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; // Men, Women, Kids

        public virtual ICollection<Product> Products { get; set; } 
            = new HashSet<Product>();
    }
}