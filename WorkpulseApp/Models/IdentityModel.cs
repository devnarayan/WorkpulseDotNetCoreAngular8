using CORTNE.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace CORTNE.Models
{
    public class IdentityModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }

    public class SecurityIdentityModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public List<RoleModel> UserRoles { get; set; }
        public string Token { get; set; }
    }

    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }


    }
}
