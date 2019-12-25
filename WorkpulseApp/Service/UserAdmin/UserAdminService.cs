using WorkpulseApp.Models;
using WorkpulseApp.Repository.UserAdmin;
using WorkpulseApp.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.Service.UserAdmin
{
    public class UserAdminService :IUserAdminService
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IUserAdminRepository _userAdminRepository;

        public UserAdminService(IUserAdminRepository userAdminRepository)
        {
            _userAdminRepository = userAdminRepository;
        }
        /// <summary>
        /// Get the current logged in user to identity authetication done or not.
        /// </summary>
        /// <returns></returns>
        public async Task<IdentityModel> GetCurrentUserAsync(HttpContext httpContext)
        {
            return await _userAdminRepository.GetCurrentUserAsync(httpContext);
        }

        /// <summary>
        /// Get the current logged in user to identity authetication done or not.
        /// </summary>
        /// <returns></returns>
        public async Task<SecurityIdentityModel> GetCurrentSecurityUserAsync(HttpContext httpContext)
        {
            return await _userAdminRepository.GetCurrentSecurityUserAsync(httpContext);
        }

        /// <summary>
        /// Search the all users which are registered to WorkpulseApp.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<AspNetUsers>> GetSearchUsersAync(string search)
        {
            return await _userAdminRepository.GetSearchUsersAync(search);
        }
        /// <summary>
        /// get assigned counties
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<CountyModel>> GetCountiesAsync(string userName)
        {
            return await _userAdminRepository.GetCountiesAsync(userName);
        }

        /// <summary>
        /// Get the Roles 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<RoleModel>> GetRolesAsync(string userName)
        {
            return await _userAdminRepository.GetRolesAsync(userName);
        }
        public async Task<List<RoleModel>> UpdateUserRoleAsync(List<RoleModel> userRoles, string userName, List<CountyModel> userCounties, string updatedBy)
        {
            return await _userAdminRepository.UpdateUserRoleAsync(userRoles, userName, userCounties, updatedBy);
        }
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsersAsync()
        {
            return await _userAdminRepository.GetAspNetUsersAsync();
        }
        public async Task<ActionResult<AspNetUsers>> GetAspNetUsersByIdAsync(string id)
        {
            return await _userAdminRepository.GetAspNetUsersByIdAsync(id);
        }

        public async Task<List<UserManagementAuditHistoryViewModel>> GetUserManagementAuditHistoryAsync(UserManagementAuditHistoryViewModel searchCriteria)
        {
            return await _userAdminRepository.GetUserManagementAuditHistoryAsync(searchCriteria);
        }
    }
}
