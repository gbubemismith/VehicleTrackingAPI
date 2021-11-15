using System;

namespace Core.Dtos
{
    public class PositionForListDto
    {
        public Guid DeviceId { get; set; }
        public Guid UserId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}