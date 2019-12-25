using AutoMapper;
using WorkpulseApp.Models;
using WorkpulseApp.ViewModel;

namespace WorkpulseApp
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AspNetUsers, AspNetUserDto>();
        }
    }
}
