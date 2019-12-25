using AutoMapper;
using CORTNE.Models;
using CORTNE.ViewModel;

namespace CORTNE
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AspNetUsers, AspNetUserDto>();
        }
    }
}
