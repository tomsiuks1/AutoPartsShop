namespace Models.DTOs
{
    public class CreateCommentDto
    {
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
    }
}