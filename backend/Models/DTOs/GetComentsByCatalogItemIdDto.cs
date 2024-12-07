namespace Models.DTOs
{
    public class GetComentsByCatalogItemIdDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { get; set; }
        public Guid UserId { get; set; }
    }
}