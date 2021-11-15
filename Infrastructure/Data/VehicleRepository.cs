using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(VehicleDbContext context) : base(context)
        {

        }

        //returns 
        public async Task<Vehicle> GetVehicleById(Guid id)
        {
            return await _context.Vehicles
                .Include(p => p.Device)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /** 
            Using linq lambda joins

            sample sql::
            select *
            from Vehicles 
            where UserId = '08d908c8-b436-4004-8e51-3d27bc203b58'
            ANd  id = (select Vehicleid from Devices where Id = '82be412e-df03-48e3-802c-2f51ce6d00ea')
        
         **/
        public async Task<int> ConfirmVehicle(Guid userId, Guid deviceId)
        {
            return await _context.Vehicles
                        .Join(_context.Devices, v => v.Id, d => d.VehicleId,
                        (vehicle, device) => new { resVehicle = vehicle, resDevice = device })
                        .Where(x => x.resVehicle.UserId == userId && x.resDevice.Id == deviceId)
                        .CountAsync();
        }

        public async Task<VehicleCurrentDto> GetCurrentPosition(Guid userId, Guid deviceId)
        {

            var recent = await _context.Vehicles
                            .Join(_context.Devices, v => v.Id, d => d.VehicleId,
                            (vehicle, device) => new { Vehicle = vehicle, Device = device })
                            .Join(_context.Locations, d => d.Device.Id, l => l.DeviceId,
                            (device, location) => new { Device = device, Location = location, Vehicle = device.Vehicle })
                            .Select(x => new
                            {
                                VehicleId = x.Vehicle.Id,
                                VehicleName = x.Vehicle.VehicleName,
                                UserId = x.Vehicle.UserId,
                                DeviceId = x.Location.DeviceId,
                                Lat = x.Location.Latitude,
                                Long = x.Location.Longitude,
                                TimeStamp = x.Location.CreateDt
                            })
                            .Where(p => p.UserId == userId && p.DeviceId == deviceId)
                            .OrderByDescending(p => p.TimeStamp)
                            .FirstOrDefaultAsync();


            return new VehicleCurrentDto
            {
                DeviceId = recent.DeviceId,
                UserId = recent.UserId,
                Latitude = recent.Lat,
                Longitude = recent.Long,
                TimeStamp = recent.TimeStamp
            };
        }

        public IQueryable<VehicleCurrentDto> GetPositionsRange(LocationsParams locationsParams)
        {
            var recent = _context.Vehicles
                            .Join(_context.Devices, v => v.Id, d => d.VehicleId,
                            (vehicle, device) => new { Vehicle = vehicle, Device = device })
                            .Join(_context.Locations, d => d.Device.Id, l => l.DeviceId,
                            (device, location) => new { Device = device, Location = location, Vehicle = device.Vehicle })
                            .Select(x => new VehicleCurrentDto
                            {
                                UserId = x.Vehicle.UserId,
                                DeviceId = x.Location.DeviceId,
                                Latitude = x.Location.Latitude,
                                Longitude = x.Location.Longitude,
                                TimeStamp = x.Location.CreateDt
                            })
                            .Where(p => p.UserId == locationsParams.UserId
                                && p.DeviceId == locationsParams.DeviceId
                                && p.TimeStamp >= locationsParams.StartDt
                                && p.TimeStamp <= locationsParams.EndDt
                                )
                            .OrderByDescending(p => p.TimeStamp)
                            .AsQueryable();

            return recent;
        }
    }
}