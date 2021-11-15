using System;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppUserRole : IdentityUserRole<Guid>
    {
        public User User { get; set; }
        public AppRole Role { get; set; }
    }
}