namespace Models.DTOs
{
    public class BasketItemDto
    {
        public Guid CatalogItemId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string PictureUrl { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
    }
}
