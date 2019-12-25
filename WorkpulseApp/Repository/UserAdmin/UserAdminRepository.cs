using CORTNE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CORTNE.ViewModel;
using Microsoft.EntityFrameworkCore;
using CORTNE.Exception;
using CORTNE.Helpers;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CORTNE.Repository.UserAdmin
{
    public class UserAdminRepository : IUserAdminRepository
    {
        private readonly CORTNEDEVContext _context;
        private readonly AppSettings _appSettings;

        public UserAdminRepository(CORTNEDEVContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        /// <summary>
        /// Get the current logged in user to identity authetication done or not.
        /// </summary>
        /// <returns></returns>
        public async Task<IdentityModel> GetCurrentUserAsync(HttpContext httpContext)
        {
            try
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    var identity = httpContext.User.Identity as ClaimsIdentity; // Azure AD V2 endpoint specific
                    string name = identity.Claims.FirstOrDefault(c => c.Type == "name")?.Value;

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_appSettings.API_Secret);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = identity,
                        Expires = DateTime.UtcNow.AddDays(7),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);

                    IdentityModel model = new IdentityModel
                    {
                        Name = name,
                        UserName = identity.Name,
                        Token = tokenHandler.WriteToken(token)
                    };


                    return await Task.FromResult<IdentityModel>(model);
                }
                return await Task.FromResult<IdentityModel>(null);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in UserAdminRepository.GetCurrentUserAsync({nameof(httpContext)})", ex);
            }
        }

        /// <summary>
        /// Get the current logged in user to identity authetication done or not.
        /// </summary>
        /// <returns></returns>
        public async Task<SecurityIdentityModel> GetCurrentSecurityUserAsync(HttpContext httpContext)
        {
            try
            {
                if (httpContext.User.Identity.IsAuthenticated)
                {
                    var identity = httpContext.User.Identity as ClaimsIdentity; // Azure AD V2 endpoint specific                    
                    string name = identity.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
                    var tokenString = string.Empty;

                    try
                    {
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.API_Secret));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(new Claim[]
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, name),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            }),
                            Expires = DateTime.UtcNow.AddMinutes(120),
                            SigningCredentials = creds
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        tokenString = tokenHandler.WriteToken(token);
                    }
                    catch (System.Exception ex)
                    {
                        var exceptionString = ex.ToString();
                    }

                    SecurityIdentityModel model = new SecurityIdentityModel
                    {
                        Name = name,
                        UserName = identity.Name,
                        UserRoles = await GetRolesAsync(identity.Name).ConfigureAwait(false),
                        Token = tokenString
                    };


                    return await Task.FromResult<SecurityIdentityModel>(model);
                }
                return await Task.FromResult<SecurityIdentityModel>(null);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in UserAdminRepository.GetCurrentUserAsync({nameof(httpContext)})", ex);
            }
        }

        /// <summary>
        /// Search the all users which are registered to CORTNE.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public async Task<List<AspNetUsers>> GetSearchUsersAync(string search)
        {
            try
            {
                var timerRecords = await _context.AspNetUsers.Where(st => st.UserName.Contains(search) || st.Email.Contains(search)).ToListAsync().ConfigureAwait(false);

                return await Task.FromResult<List<AspNetUsers>>(timerRecords);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in UserAdminRepository.GetSearchUsersAync({nameof(search)})", ex);
            }
        }
        /// <summary>
        /// Get User Counties
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<CountyModel>> GetCountiesAsync(string userName)
        {
            try
            {
                var rList = new List<CountyModel>();
                var r = await _context.LocationLk.ToListAsync().ConfigureAwait(false);
                return await Task.FromResult<List<CountyModel>>(rList);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in UserAdminRepository.GetRolesAsync({nameof(userName)})", ex);
            }
        }

        /// <summary>
        /// Search the user in active directory and also find user registered in CORTNE or not.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<RoleModel>> GetRolesAsync(string userName)
        {
            try
            {
                List<RoleModel> rList = new List<RoleModel>();
                var r = await _context.AspNetRoles.ToListAsync().ConfigureAwait(false);


                var user = await _context.AspNetUsers.Where(st => st.UserName == userName).FirstOrDefaultAsync().ConfigureAwait(false);
                if (user != null)
                {
                    var userRoles = _context.AspNetUserRoles.Where(st => st.UserId == user.Id).ToAsyncEnumerable();
                    if (r != null && r.Count() > 0)
                    {
                        foreach (var item in r)
                        {
                            RoleModel rm = new RoleModel
                            {
                                RoleId = item.Id,
                                RoleName = item.Name,
                                GroupName = item.Groups,
                                Assigned = await userRoles.Any(x => x.RoleId == item.Id)
                            };
                            rList.Add(rm);
                        }
                    }
                }

                return await Task.FromResult<List<RoleModel>>(rList);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in UserAdminRepository.GetRolesAsync({nameof(userName)})", ex);
            }
        }
        public async Task<List<RoleModel>> UpdateUserRoleAsync(List<RoleModel> userRoles, string userName, List<CountyModel> userCounties, string updatedBy)
        {
            try
            {
                List<RoleModel> rList = new List<RoleModel>();


                var roleIds = userRoles.Where(st => st.Assigned == true).Select(st => st.RoleId).ToList();

                var user = await _context.AspNetUsers.Where(st => st.UserName == userName).FirstOrDefaultAsync().ConfigureAwait(false);
                if (user != null)
                {
                    var userRoleList = await _context.AspNetUserRoles.Where(st => st.UserId == user.Id).ToListAsync().ConfigureAwait(false);
                    if (userRoleList == null)
                    {
                        userRoleList = new List<AspNetUserRoles>();
                    }

                    //Remove unassigned roles.
                    var removableRoles = userRoleList.Where(st => !roleIds.Contains(st.RoleId)).ToList();
                    if (removableRoles.Count > 0)
                    {
                        _context.AspNetUserRoles.RemoveRange(removableRoles);
                        //Write Removed roles to History
                        foreach (var role in removableRoles)
                        {
                            _context.UserManagementAuditHistories.Add(new UserManagementAuditHistory { ActionType = "Remove", RoleId = role.RoleId, UpdatedBy = updatedBy, UpdatedDate = DateTime.Now, UserName = userName });
                        }
                        await _context.SaveChangesAsync().ConfigureAwait(false);
                    }

                    foreach (var roleId in roleIds)
                    {
                        if (userRoleList.Where(st => st.RoleId.Contains(roleId)).Count() == 0)
                        {
                            // Add it
                            _context.AspNetUserRoles.Add(new AspNetUserRoles { RoleId = roleId, UserId = user.Id });
                            //Write Added roles to History
                            _context.UserManagementAuditHistories.Add(new UserManagementAuditHistory { ActionType = "Add", RoleId = roleId, UpdatedBy = updatedBy, UpdatedDate = DateTime.Now, UserName = userName });
                        }
                    }


                    // Get the Role ID for the Debit Memo Viewer to reord
                    var debitMemoViewerRoleID = await _context.AspNetRoles.Where(x => x.Name == "Debit Memo Viewer" && x.Groups == "Debit Memo").Select(x => x.Id).SingleAsync();
                    var locationCodes = userCounties.Select(u => u.CountyId).ToList();

                    await _context.SaveChangesAsync().ConfigureAwait(false);

                }

                return await Task.FromResult<List<RoleModel>>(rList);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in UserAdminRepository.UpdateUserRoleAsync({nameof(userRoles)},{nameof(userName)})", ex);
            }
        }
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsersAsync()
        {
            try
            {
                return await _context.AspNetUsers.ToListAsync().ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in UserAdminRepository.GetAspNetUsersAsync()", ex);
            }
        }

        public async Task<ActionResult<AspNetUsers>> GetAspNetUsersByIdAsync(string id)
        {
            try
            {
                return await _context.AspNetUsers.FindAsync(id).ConfigureAwait(false);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in UserAdminRepository.GetAspNetUsersByIdAsync()", ex);
            }
        }

        public async Task<List<UserManagementAuditHistoryViewModel>> GetUserManagementAuditHistoryAsync(UserManagementAuditHistoryViewModel searchCriteria)
        {
            
            var result = from umah in _context.UserManagementAuditHistories
                            join anr in _context.AspNetRoles on umah.RoleId equals anr.Id into grpjoin_umah_anr
                                from anr in grpjoin_umah_anr.DefaultIfEmpty()
                            join loc in _context.LocationLk on umah.LocationCode equals loc.Code into grpjoin_umah_loc
                                from loc in grpjoin_umah_loc.DefaultIfEmpty()
                         where umah.UpdatedDate.Date >= searchCriteria.ModifiedStartDate && umah.UpdatedDate.Date <= searchCriteria.ModifiedEndDate
                         orderby umah.UpdatedDate descending
                         select new UserManagementAuditHistoryViewModel()
                         {
                             RowId = umah.RowId,
                             UserName = umah.UserName,
                             UpdatedBy = umah.UpdatedBy,
                             UpdatedDate = umah.UpdatedDate,
                             ComponentName = anr.Groups,
                             RoleId = anr.Name,
                             ActionType = umah.ActionType,
                             LocationCode = loc.Code,
                             CountyName = loc.Name
                         };

            if(searchCriteria.ComponentName != null)
            {
                if(!searchCriteria.ComponentName.Equals("All"))
                    result = result.Where(x => x.ComponentName.Contains(searchCriteria.ComponentName));
            }

            if(searchCriteria.UserName != null)
            {
                result = result.Where(x => x.UserName.ToUpper().Contains(searchCriteria.UserName.ToUpper()));
            }

            return await Task.FromResult<List<UserManagementAuditHistoryViewModel>>(result.ToList());

            }
    }
}
