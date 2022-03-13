using Microsoft.AspNetCore.Identity;

namespace BlogEngine.Data.Identity
{

    public class AppUser : IdentityUser
    {
        // Add additional profile data for application users by adding properties to this class
        public int AuthorId { get; set; }
    }
}
