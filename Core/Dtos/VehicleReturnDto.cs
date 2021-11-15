using System;

namespace Core.Dtos
{
    public class VehicleReturnDto
    {
        public Guid Id { get; set; }
        public string VehicleName { get; set; }
        public string RegistrationNumber { get; set; }
        public Guid DeviceId { get; set; }
        public Guid UserId { get; set; }
    }
}