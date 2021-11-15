using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}