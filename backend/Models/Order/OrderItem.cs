namespace Models.Orders
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public CatalogItemOrdered ItemOrdered { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
