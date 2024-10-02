using Microsoft.EntityFrameworkCore;

namespace Models.Orders
{
    [Owned]
    public class CatalogItemOrdered
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
    }
}
