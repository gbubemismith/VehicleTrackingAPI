using System;
using Core.Entities.Identity;

namespace Core.Entities
{
    public class Vehicle : BaseEntity
    {
        public string VehicleName { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime CreateDt { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }

        /* Ef Relation*/
        public Device Device { get; set; }

    }
}