﻿
using System.Text.Json.Serialization;

namespace Models.Catalog
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Price { get; set; }
        public string PictureUrl { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int QuantityInStock { get; set; }
        public string PublicId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Guid CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
