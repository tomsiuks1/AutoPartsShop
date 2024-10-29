namespace Models.DTOs
{
    public class CatalogItemCreateDto
    {
        public string Name { get; set; }
        public Guid CategoryId { get; set; }
    }
}