using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.services;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VehicleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        public async Task<Vehicle> GetVehicleById(Guid id)
        {
            return await _unitOfWork.Vehicles.GetVehicleById(id);
        }

        public async Task<int> ConfirmVehicle(Guid userId, Guid deviceId)
        {
            return await _unitOfWork.Vehicles.ConfirmVehicle(userId, deviceId);
        }

        /** 
            - Assuming a user has registerd and logged into the client
            - We get his userid and details of his/her vehicle, device
                and then register
            - Unit of work approach is used to manage transactions and rollback better,
                hence, the use of GUID so ID is generated from code instead of db
            
        **/
        public async Task<VehicleReturnDto> RegisterVehicle(VehicleDto vehicleDto, Guid userId)
        {
            //map dto to entity object
            var vehicle = VehicleMapper.Mapper.Map<Vehicle>(vehicleDto);
            vehicle.UserId = userId;
            vehicle.CreateDt = DateTime.Now;
            vehicle.Id = Guid.NewGuid();

            //create vehicle
            _unitOfWork.Vehicles.Add(vehicle);

            var device = new Device
            {
                Id = Guid.NewGuid(),
                DeviceName = vehicleDto.DeviceName,
                VehicleId = vehicle.Id,
                CreateDt = DateTime.Now
            };

            //create device
            _unitOfWork.Devices.Add(device);

            var resp = await _unitOfWork.CompleteAsync();

            return VehicleMapper.Mapper.Map<VehicleReturnDto>(vehicle);
        }


        public async Task<bool> RecordVehiclePosition(VehiclePositionDto positionDto)
        {
            //map dto to entity object
            var location = VehicleMapper.Mapper.Map<Location>(positionDto);
            location.CreateDt = DateTime.Now;
            location.Id = Guid.NewGuid();

            _unitOfWork.Locations.Add(location);

            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<VehicleCurrentDto> GetCurrentPosition(Guid userId, Guid deviceId)
        {
            // get current position of vehicle
            var location = await _unitOfWork.Vehicles.GetCurrentPosition(userId, deviceId);
            

            return location;
        }

        //Result is paginated to improve performance
        public async Task<PagedList<VehicleCurrentDto>> GetPositionsRange(LocationsParams locationsParams)
        {
            //get locations list
            var locationQuery = _unitOfWork.Vehicles.GetPositionsRange(locationsParams);

            return await ObjectPagedList<VehicleCurrentDto>.CreateAsync(locationQuery, locationsParams.PageNumber, locationsParams.PageSize);
        }


    }
}