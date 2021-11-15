using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class DeviceRepsoitory : GenericRepository<Device>, IDeviceRepository
    {
        public DeviceRepsoitory(VehicleDbContext context) : base(context)
        {

        }
    }
}