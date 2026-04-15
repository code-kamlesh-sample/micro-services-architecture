using Microsoft.AspNetCore.Identity;

namespace Microservices.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
