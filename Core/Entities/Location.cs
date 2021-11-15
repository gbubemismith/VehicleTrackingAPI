using System;

namespace Core.Entities
{
    public class Location : BaseEntity
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime CreateDt { get; set; }
        public Guid DeviceId { get; set; }
        public Device Device { get; set; }
    }
}