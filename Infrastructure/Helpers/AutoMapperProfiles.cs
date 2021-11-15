using AutoMapper;
using Core.Dtos;
using Core.Entities;

namespace Infrastructure.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<VehicleDto, Vehicle>();

            CreateMap<Vehicle, VehicleReturnDto>()
                    .ForMember(dest => dest.DeviceId,
                    opt => opt.MapFrom(src => src.Device.Id));

            CreateMap<VehiclePositionDto, Location>();



        }
    }
}