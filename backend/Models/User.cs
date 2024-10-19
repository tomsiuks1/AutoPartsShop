using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class User : IdentityUser<Guid>
    {
        public string DisplayName { get; set; }
        public UserAddress Address { get; set; }
    }
}