using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<Vehicle> GetVehicleById(Guid id);
        Task<int> ConfirmVehicle(Guid userId, Guid deviceId);
        Task<VehicleCurrentDto> GetCurrentPosition(Guid userId, Guid deviceId);
        IQueryable<VehicleCurrentDto> GetPositionsRange(LocationsParams locationsParams);
    }
}