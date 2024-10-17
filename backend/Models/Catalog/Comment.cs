using System.Text.Json.Serialization;

namespace Models.Catalog
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CatalogItemId { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public CatalogItem CatalogItem { get; set; }
        public string DisplayName { get; set; }
    }
}