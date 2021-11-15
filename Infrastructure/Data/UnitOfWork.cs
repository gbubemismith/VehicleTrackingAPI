using System.Threading.Tasks;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VehicleDbContext _context;
        public UnitOfWork(VehicleDbContext context)
        {
            _context = context;
            Vehicles = new VehicleRepository(_context);
            Devices = new DeviceRepsoitory(_context);
            Locations = new LocationRepository(_context);
        }

        public IVehicleRepository Vehicles { get; private set; }
        public IDeviceRepository Devices { get; private set; }
        public ILocationRepository Locations { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}