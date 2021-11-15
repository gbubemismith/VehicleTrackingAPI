using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Dtos;
using Core.Interfaces.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /**
        ** Author - Gbubemi Smith
        ** Client - Seven Peaks

        ** Application is built using clean architecture, SOLID principles, repository pattern,
           EF core, Automapper, Ef Migrations, MySql, unit of work pattern(UoW), google api,
           JWT token for authorization, Identity Manager
        ** Exceptions are handled with exception middleware to make the code cleaner

     **/


    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IHttpServices _httpServices;
        private readonly IMapper _mapper;
        public VehicleController(IVehicleService vehicleService, IHttpServices httpServices, IMapper mapper)
        {
            _mapper = mapper;
            _httpServices = httpServices;
            _vehicleService = vehicleService;

        }

        [HttpGet("{id}", Name = "GetVehicle")]
        public async Task<IActionResult> GetVehicle(Guid id)
        {
            var vehicle = await _vehicleService.GetVehicleById(id);

            if (vehicle == null)
                return NotFound(new ApiResponse(404));

            return Ok(vehicle);
        }


        /** 
            - Only registered and logged in users can resgister a vehicle
            - Method registers a vehicle associates device with it

            {
                "vehicleName": "Toyota",
                "registrationNumber": "TY1234",
                "deviceName": "Tracker 1"
            }
        
        **/
        [HttpPost("RegisterVehicle")]
        public async Task<IActionResult> RegisterVehicle(VehicleDto vehicleDto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var createdVehicle = await _vehicleService.RegisterVehicle(vehicleDto, Guid.Parse(currentUserId));

            return CreatedAtRoute("GetVehicle", new { Controller = "Vehicle", id = createdVehicle.Id }, createdVehicle);
        }


        /**
            sample request:
            {
                "deviceId": "0babde7f-3226-4225-8bbd-2617650ae360",
                "longitude": 6.478763971153605,
                "latitude": 3.5839634464857317
            }
        
        **/
        [HttpPost("{userId}/RecordPosition")]
        public async Task<IActionResult> RecordPosition(Guid userId, VehiclePositionDto vehiclePosition)
        {
            //ensure logged in user is sending the request
            if (userId != Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized(new ApiResponse(401));

            //check to if user and device really own vehicle
            var confirmResult = await _vehicleService.ConfirmVehicle(userId, vehiclePosition.DeviceId);

            if (confirmResult == 0)
                return BadRequest(new ApiResponse(400, "Vehicle does not have a match with your user or device"));

            var status = await _vehicleService.RecordVehiclePosition(vehiclePosition);

            //successfully recorded position
            if (status)
                return NoContent();


            return BadRequest(new ApiResponse(400));

        }

        // api/v1/vehicle/currentposition?userid=08d908c8-b436-4004-8e51-3d27bc203b58&deviceId=0babde7f-3226-4225-8bbd-2617650ae360
        /** 
        Assumption
            * Admin user is seeded into the system with email = admin@test.com, password = Pa$$w0rd
            * Only a registered and logged in admin user is authorized, any other would return a forbidden

        **/
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("CurrentPosition")]
        public async Task<IActionResult> GetVehicleCurrentPosition(Guid userId, Guid deviceId)
        {
            var resp = await _vehicleService.GetCurrentPosition(userId, deviceId);

            if (resp == null)
                return NotFound(new ApiResponse(404, "No position for vehicle"));

            //bonus -- Added locality using google geocode api
            var locality = await _httpServices.LocalityName(resp.Latitude, resp.Longitude);

            resp.Location = locality;

            return Ok(resp);
        }

        //api/v1/Vehicle/GetVehiclePositionByRange?UserId=08d908c8-b436-4004-8e51-3d27bc203b58&DeviceId=0babde7f-3226-4225-8bbd-2617650ae360&StartDt=&EndDt=
        /**
            Assumption - Gets vehicle positions by date range
            * The result is paginated 
        **/
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetVehiclePositionByDate")]
        public async Task<IActionResult> GetVehiclePositionByRange([FromQuery] LocationsParams locationsParams)
        {
            var resp = await _vehicleService.GetPositionsRange(locationsParams);

            if (resp == null)
                return NotFound(new ApiResponse(404, "No position for vehicle"));

            //re-map to exclude paginated details. I return paginated details in the response header
            var locations = _mapper.Map<IEnumerable<PositionForListDto>>(resp);

            //add paginated details to response headers
            Response.AddPagination(resp.CurrentPage, resp.PageSize, resp.TotalCount, resp.TotalPages);


            return Ok(locations);
        }

    }
}