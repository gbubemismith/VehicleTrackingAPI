using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.Identity
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}