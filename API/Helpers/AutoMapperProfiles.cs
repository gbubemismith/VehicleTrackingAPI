using AutoMapper;
using Core.Dtos;
using Core.Entities.Identity;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<VehicleCurrentDto, PositionForListDto>();
            CreateMap<RegisterUserDto, User>().ReverseMap();
        }
    }
}