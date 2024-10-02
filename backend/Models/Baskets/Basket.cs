using Models.Catalog;

namespace Models.Baskets
{
    public class Basket
    {
        public Guid Id { get; set; }
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; } = new List<BasketItem>();
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }

        public void AddItem(CatalogItem catalogItem, int quantity)
        {
            if (Items.All(item => item.CatalogItemId != catalogItem.Id))
            {
                Items.Add(new BasketItem { CatalogItem = catalogItem, Quantity = quantity });
                return;
            }

            var existingItem = Items.FirstOrDefault(item => item.CatalogItemId == catalogItem.Id);
            if (existingItem != null) existingItem.Quantity += quantity;
        }

        public void RemoveItem(Guid catalogItemId, int quantity = 1)
        {
            var item = Items.FirstOrDefault(basketItem => basketItem.CatalogItemId == catalogItemId);
            if (item == null) return;
            item.Quantity -= quantity;
            if (item.Quantity == 0) Items.Remove(item);
        }
    }
}
