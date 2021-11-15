using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class VehiclePositionDto
    {
        [Required]
        public Guid DeviceId { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

    }
}