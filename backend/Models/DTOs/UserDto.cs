namespace Models.DTOs
{
    public class UserDto
    {
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public BasketDto Basket { get; set; }
        public Guid Id { get; set; }
    }
}
