using CORTNE.Models;
using CORTNE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORTNE.Service.UserAdmin
{
    public interface IUserAdminService
    {
        /// <summary>
        /// Get the current logged in user to identity authetication done or not.
        /// </summary>
        /// <returns></returns>
        Task<IdentityModel> GetCurrentUserAsync(HttpContext httpContext);

        /// <summary>
        /// Get the current logged in user to identity authetication done or not.
        /// </summary>
        /// <returns></returns>
        Task<SecurityIdentityModel> GetCurrentSecurityUserAsync(HttpContext httpContext);

        /// <summary>
        /// Search the all users which are registered to CORTNE.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        Task<List<AspNetUsers>> GetSearchUsersAync(string search);

        /// <summary>
        /// Get the Roles 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<List<RoleModel>> GetRolesAsync(string userName);
        /// <summary>
        /// Get User Counties 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<List<CountyModel>> GetCountiesAsync(string userName);

        /// <summary>
        /// Update the Roles 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<List<RoleModel>> UpdateUserRoleAsync(List<RoleModel> userRoles, string userName, List<CountyModel> userCounties, string updatedBy);

        /// <summary>
        /// Get the AspNetUsers
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsersAsync();

        /// <summary>
        /// Get the AspNetUsers by Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ActionResult<AspNetUsers>> GetAspNetUsersByIdAsync(string id);

        /// <summary>
        /// Get the AspNetUsers by Id
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>List of UserManagementAuditHistory</returns>
        Task<List<UserManagementAuditHistoryViewModel>> GetUserManagementAuditHistoryAsync(UserManagementAuditHistoryViewModel searchCriteria);
    }
}
