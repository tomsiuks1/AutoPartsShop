namespace Models.Catalog
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<CatalogItem> CatalogItems { get; set; }
    }
}