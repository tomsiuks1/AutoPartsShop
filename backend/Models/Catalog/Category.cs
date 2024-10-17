namespace Models.Catalog
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CatalogItem> CatalogItems { get; set; } = new List<CatalogItem>();
    }
}