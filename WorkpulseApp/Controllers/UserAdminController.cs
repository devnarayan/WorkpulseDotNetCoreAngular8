using AutoMapper;
using WorkpulseApp.Helpers;
using WorkpulseApp.Models;
using WorkpulseApp.Service.UserAdmin;
using WorkpulseApp.ViewModel;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TestGraphApi.Models;

namespace WorkpulseApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserAdminController : ControllerBase
    {
        private readonly CORTNEDEVContext _context;
        private IMapper _mapper;
        private readonly IUserAdminService _userAdminService;
        private readonly CommonHelper _commonHelper;

        private static readonly ILog _log = LogManager.GetLogger(typeof(UserAdminController));

        public UserAdminController(CORTNEDEVContext context, IMapper mapper,
            IServiceProvider serviceProvider, IUserAdminService userAdminService, CommonHelper commonHelper)
        {
            _context = context;
            _mapper = mapper;
            _userAdminService = userAdminService;
            _commonHelper = commonHelper;
        }

        /// <summary>
        /// Get the current logged in user to identity authetication done or not.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCurrentUser()
        {            
            var user = HttpContext.User.Identity.Name;
            var aidentityModel = await _userAdminService.GetCurrentUserAsync(HttpContext).ConfigureAwait(false);

            if(aidentityModel == null)
            {
                return Ok("");
            }
            _log.Info("GetCurrentUser " + user.ToString() + ":- " + DateTime.Now.ToString() + " success");
            return Ok(aidentityModel);
        }

        /// <summary>
        /// Get the current logged in user to identity authetication done or not.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> GetCurrentSecurityUser()
        {            
            var user = HttpContext.User.Identity.Name;
            var aidentityModel = await _userAdminService.GetCurrentSecurityUserAsync(HttpContext).ConfigureAwait(false);

            if (aidentityModel == null)
            {
                return Ok("");
            }
            _log.Info("GetCurrentSecurityUser " + user.ToString() + ":- " + DateTime.Now.ToString() + " success");
            return Ok(aidentityModel);
        }

        /// <summary>
        /// Search the all users which are registered to WorkpulseApp.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("SearchUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetSearchUsers(string search)
        {
            try
            {
                var timerRecords = await _userAdminService.GetSearchUsersAync(search).ConfigureAwait(false);
                if (timerRecords == null)
                {
                    return NotFound();
                }
                var timerDto = _mapper.Map<List<AspNetUserDto>>(timerRecords);

                return Ok(timerDto);
            }
            catch (System.Exception ex)
            {

                ErrorViewModel model = new ErrorViewModel();
                model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                throw ex;
            }
        }

        /// <summary>
        /// Get all users which are registered to WorkpulseApp.
        /// </summary>        
        /// <returns></returns>
        [Route("GetAllUsers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetAllUsers()
        {
            try
            {
                var timerRecordsAsync = await _context.AspNetUsers.ToListAsync().ConfigureAwait(false);
                var timerRecords = timerRecordsAsync.OrderBy(u => u.UserName).ToList();
                if (timerRecords == null)
                {
                    return NotFound();
                }
                var timerDto = _mapper.Map<List<AspNetUserDto>>(timerRecords);

                return Ok(timerDto);
            }
            catch (System.Exception ex)
            {

                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                throw ex;
            }
        }

        /// <summary>
        /// Search the user in active directory and also find user registered in WorkpulseApp or not.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [Route("SearchUserInAD")]
        [Authorize]
        public async Task<ActionResult> GetSearchUserInAD(string userName)
        {
            var identity = User.Identity as ClaimsIdentity;
            string accessToken = identity.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone")?.Value;

            var userInfo = GraphService.UserProfile(accessToken, userName);
            if (userInfo != null && userInfo.id != null)
            {
                var user = await _context.AspNetUsers.Where(st => st.UserName == userName).FirstOrDefaultAsync();
                if (user == null)
                {
                    return Ok(new { success = true, message = "User present in AD network." });
                }
                else
                {
                    return Ok(new { success = false, message = "User already registered with WorkpulseApp" });
                }
            }
            else return Ok(new { success = false, message = "User not found in Active Directory." });
        }

        [Route("AddNewUser")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetAddNewUser([FromBody] VMUpdateUserRole vm, string userName)
        {
            AspNetUsers aspNetUsers = new AspNetUsers
            {
                Id = Guid.NewGuid().ToString(),
                Email = userName,
                UserName = userName,
                EmailConfirmed = false,
                AccessFailedCount = 0,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
            };
            try
            {
                await _context.AspNetUsers.AddAsync(aspNetUsers);
                await _context.UserManagementAuditHistories.AddAsync(new UserManagementAuditHistory { ActionType = "Add", UpdatedBy = vm.UpdatedBy, UpdatedDate = DateTime.Now, UserName = userName });
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return Ok(true);
            }
            catch (DbUpdateException ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);

                if (AspNetUsersExists(aspNetUsers.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw ex;
                }
            }
            catch (System.Exception ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                return BadRequest(ex);
            }
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<RoleModel>>> GetRoles(string userName)
        {
            return await _userAdminService.GetRolesAsync(userName).ConfigureAwait(false);
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<CountyModel>>> GetCounties(string userName)
        {
            return await _userAdminService.GetCountiesAsync(userName).ConfigureAwait(false);
        }

        [Route("GetGroups")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<string>>> GetGroups()
        {
            var groupList = await _context.AspNetRoles.Select(x => x.Groups).Distinct().ToListAsync().ConfigureAwait(false);
            groupList.Insert(0, "All");
            return groupList;
        }

        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<RoleModel>>> GetCurrentUserRoles(string userName)
        {
            return await _userAdminService.GetRolesAsync(userName).ConfigureAwait(false);
        }


        [HttpGet("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<bool>> GetUserHasRole(string userName, string roleId)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (!string.IsNullOrEmpty(roleId))
                {
                    string userId = _context.AspNetUsers.Where(st => st.UserName == userName).FirstOrDefault().Id;
                    if (!string.IsNullOrEmpty(userId))
                    {

                        return await _context.AspNetUserRoles.AnyAsync(u => u.UserId == userId && u.RoleId == roleId).ConfigureAwait(false);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;            
        }

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<RoleModel>>> UpdateUserRole([FromBody] VMUpdateUserRole vm, string userName)
        {
            return await _userAdminService.UpdateUserRoleAsync(vm.UserRoles, userName, vm.UserCounties, vm.UpdatedBy);
        }

        [Route("RemoveRoles")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> GetRemoveRoles([FromBody] VMUpdateUserRole vm, string userName)
        {
            try
            {
                var user = await _context.AspNetUsers.Where(st => st.UserName == userName).FirstOrDefaultAsync().ConfigureAwait(false);
                if (user != null)
                {
                    var userRoles = _context.AspNetUserRoles.Where(st => st.UserId == user.Id).AsEnumerable();
                    if (userRoles != null)
                    {
                        _context.AspNetUserRoles.RemoveRange(userRoles);
                        foreach (var role in userRoles)
                        {
                            _context.UserManagementAuditHistories.Add(new UserManagementAuditHistory { ActionType = "Remove", RoleId = role.RoleId, UpdatedBy = vm.UpdatedBy, UpdatedDate = DateTime.Now, UserName = userName });
                        }
                        await _context.SaveChangesAsync().ConfigureAwait(false);
                    }
                }

            }
            catch (System.Exception ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);

                return Ok(new { success = false, message = "Please try again later." });
            }
            return Ok(new { success = true, message = "User role has been removed successfully." });
        }

        private JsonResult Json(object p, object allowGet)
        {
            throw new NotImplementedException();
        }


        #region AspNetUser CRUD

        // GET: api/UserAdmin
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<AspNetUsers>>> GetAspNetUsers()
        {
            return await _userAdminService.GetAspNetUsersAsync().ConfigureAwait(false);
        }

        // GET: api/UserAdmin/5
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AspNetUsers>> GetAspNetUsers(string id)
        {
            var aspNetUsers = await _userAdminService.GetAspNetUsersByIdAsync(id).ConfigureAwait(false);

            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return aspNetUsers;
        }

        // PUT: api/UserAdmin/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PutAspNetUsers(string id, AspNetUsers aspNetUsers)
        {
            if (id != aspNetUsers.Id)
            {
                return BadRequest();
            }

            _context.Entry(aspNetUsers).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model); 

                if (!AspNetUsersExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserAdmin
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AspNetUsers>> PostAspNetUsers(AspNetUsers aspNetUsers)
        {
            _context.AspNetUsers.Add(aspNetUsers);
            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateException ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);

                if (AspNetUsersExists(aspNetUsers.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAspNetUsers", new { id = aspNetUsers.Id }, aspNetUsers);
        }

        // DELETE: api/UserAdmin/5
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<AspNetUsers>> DeleteAspNetUsers(string id)
        {
            var aspNetUsers = await _context.AspNetUsers.FindAsync(id).ConfigureAwait(false);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            _context.AspNetUsers.Remove(aspNetUsers);
            await _context.SaveChangesAsync();

            return aspNetUsers;
        }

        private bool AspNetUsersExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }

        #endregion

        [HttpPost("[action]")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<UserManagementAuditHistoryViewModel>>> SearchUserManagementAuditHistory([FromBody] UserManagementAuditHistoryViewModel vm)
        {
            return await _userAdminService.GetUserManagementAuditHistoryAsync(vm).ConfigureAwait(false);
        }
    }
}
