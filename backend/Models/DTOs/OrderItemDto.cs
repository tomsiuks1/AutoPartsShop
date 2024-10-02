﻿namespace Models.DTOs
{
    public class OrderItemDto
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public long Price { get; set; }
        public int Quantity { get; set; }
    }
}
