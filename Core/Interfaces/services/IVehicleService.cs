using System;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces.services
{
    public interface IVehicleService
    {
        Task<Vehicle> GetVehicleById(Guid id);
        Task<int> ConfirmVehicle(Guid userId, Guid deviceId);
        Task<VehicleReturnDto> RegisterVehicle(VehicleDto vehicleDto, Guid userId);
        Task<bool> RecordVehiclePosition(VehiclePositionDto positionDto);
        Task<VehicleCurrentDto> GetCurrentPosition(Guid userId, Guid deviceId);
        Task<PagedList<VehicleCurrentDto>> GetPositionsRange(LocationsParams locationsParams);
    }
}