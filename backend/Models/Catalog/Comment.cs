using System.Text.Json.Serialization;

namespace Models.Catalog
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid UserId { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public Guid CatalogItemId { get; set; }
        public string DisplayName { get; set; }
    }
}