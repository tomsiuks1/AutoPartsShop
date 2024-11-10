using Models.Catalog;

namespace Models.Baskets
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
    }
}
