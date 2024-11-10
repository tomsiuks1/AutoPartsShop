namespace Models.Catalog
{
    public class ProductFilter
    {
        public List<string> Brands { get; set; } = new List<string>();
        public List<string> Types { get; set; } = new List<string>();
    }
}
