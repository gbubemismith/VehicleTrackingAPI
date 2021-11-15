using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class VehicleDto
    {
        [Required]
        public string VehicleName { get; set; }

        [Required]
        public string RegistrationNumber { get; set; }

        [Required]
        public string DeviceName { get; set; }

    }
}