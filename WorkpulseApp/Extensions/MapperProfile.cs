using AutoMapper;
using WorkpulseApp.EFModelCORTNEDB;
using WorkpulseApp.Models;
using WorkpulseApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.Extensions
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<List<AspNetUserDto>, List<ApplicationUser>>();
        }
    }
}
