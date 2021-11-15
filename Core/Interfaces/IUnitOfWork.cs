using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IVehicleRepository Vehicles { get; }
        IDeviceRepository Devices { get; }
        ILocationRepository Locations { get; }
        Task<int> CompleteAsync();
    }
}