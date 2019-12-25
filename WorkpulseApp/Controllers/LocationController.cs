using AutoMapper;
using CORTNE.Exception;
using CORTNE.Helpers;
using CORTNE.Models;
using CORTNE.Repository;
using CORTNE.ViewModel;
using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestGraphApi.Models;


namespace CORTNE.Controllers
{
    //[Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class LocationController : ControllerBase
    {
        private readonly CORTNEDEVContext _context;
        private readonly CommonHelper _commonHelper;
        private readonly ILocationRepository _location;
        private IMapper _mapper;

        private static readonly ILog _log = LogManager.GetLogger(typeof(LocationController));
        private static string user = string.Empty;

        /// <summary>
        /// REST API controller for DebitMemo
        /// </summary>
        /// <param name="theAgreementService"></param>
        public LocationController(CORTNEDEVContext context, ILocationRepository location, IMapper mapper, CommonHelper commonHelper)
        {
            _location = location;
            _context = context;
            _mapper = mapper;
            _commonHelper = commonHelper;
        }

        /// <summary>
        /// Get all location list
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("GetLocations")]
        [AllowAnonymous]
        public async Task<ActionResult> GetLocations()
        {
            try
            {
                var receipt = await _location.GetLocationsAsync().ConfigureAwait(false);
                return Ok(receipt);
            }
            catch (System.Exception ex)
            {
                _log.Error(DateTime.Now.ToLocalTime() + ex.Message.ToString());
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                return Ok("Fail");
            }
        }
       
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<ActionResult<LocationViewModel>> CreateLocation(LocationViewModel crVM)
        {
            try
            {
                var adebitmemoResult = await _location.CreateLocationAsync(crVM).ConfigureAwait(false);
                return await Task.FromResult<LocationViewModel>(adebitmemoResult);
            }
            catch (System.Exception ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                throw ex;
            }
        }

        /// <summary>
        /// Get All States in a List
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("GetUSStates")]
        [AllowAnonymous]
        public async Task<ActionResult> GetUSStates()
        {
            try
            {
                var receipt = await _location.GetUSStatesAsync().ConfigureAwait(false);
                return Ok(receipt);
            }
            catch (System.Exception ex)
            {
                _log.Error(DateTime.Now.ToLocalTime() + ex.Message.ToString());
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                return Ok("Fail");
            }
        }

    }
}
