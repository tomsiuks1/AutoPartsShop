using Microsoft.AspNetCore.Identity;

namespace Models
{
    public class User : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public UserAddress Address { get; set; }
    }
}