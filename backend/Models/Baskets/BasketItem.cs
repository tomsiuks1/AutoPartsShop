using Models.Catalog;

namespace Models.Baskets
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Guid CatalogItemId { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}
