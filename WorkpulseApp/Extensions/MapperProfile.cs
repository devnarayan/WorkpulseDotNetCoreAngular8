using AutoMapper;
using CORTNE.EFModelCORTNEDB;
using CORTNE.Models;
using CORTNE.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORTNE.Extensions
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<List<AspNetUserDto>, List<ApplicationUser>>();
        }
    }
}
