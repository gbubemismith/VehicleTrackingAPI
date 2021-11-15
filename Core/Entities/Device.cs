using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Device : BaseEntity
    {
        public string DeviceName { get; set; }
        public Guid VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime CreateDt { get; set; }
        public ICollection<Location> Locations { get; set; }
    }
}