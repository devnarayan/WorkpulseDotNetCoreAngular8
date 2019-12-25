using AutoMapper;
using CORTNE.Exception;
using CORTNE.Helpers;
using CORTNE.Models;
using CORTNE.Service.ApplicationConfiguration;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class ApplicationConfigurationController : ControllerBase
    {
        private readonly CORTNEDEVContext _context;
        private readonly CommonHelper _commonHelper;
        private readonly IApplicationConfigurationService _service;
        private IMapper _mapper;

        private static readonly ILog _log = LogManager.GetLogger(typeof(ApplicationConfigurationController));
        private static string user = string.Empty;

        public ApplicationConfigurationController(CORTNEDEVContext context, IApplicationConfigurationService service, IMapper mapper, CommonHelper commonHelper)
        {
            _service = service;
            _context = context;
            _mapper = mapper;
            _commonHelper = commonHelper;
        }

        /// <summary>
        /// Search the all users which are registered to CORTNE.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [Route("GetDebitMemoConfigurations")]
        public async Task<ActionResult> GetDebitMemoConfigurations()
        {
            try
            {                
                var list = await _service.GetDebitMemoConfigurationsAsync().ConfigureAwait(false);                             
                return Ok(list);
            }
            catch (System.Exception ex)
            {
                _log.Error(DateTime.Now.ToLocalTime() + ex.Message.ToString());
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                return Ok("Fail");
            }
        }

        [Route("GetAOBConfigurations")]
        public async Task<ActionResult> GetAOBConfigurations()
        {
            try
            {
                var list = await _service.GetAOBConfigurationsAsync().ConfigureAwait(false);
                return Ok(list);
            }
            catch (System.Exception ex)
            {
                _log.Error(DateTime.Now.ToLocalTime() + ex.Message.ToString());
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                return Ok("Fail");
            }
        }

        [Route("GetMFCConfigurations")]
        public async Task<ActionResult> GetMFCConfigurations()
        {
            try
            {
                var list = await _service.GetMFCConfigurationsAsync().ConfigureAwait(false);
                return Ok(list);
            }
            catch (System.Exception ex)
            {
                _log.Error(DateTime.Now.ToLocalTime() + ex.Message.ToString());
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                return Ok("Fail");
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<UpdateApplicationConfigurationLkViewModelDto>> UpdateConfiguration([FromBody]UpdateApplicationConfigurationLkViewModelDto dhVM)
        {
            try
            {
                var dipayment = await _service.UpdateConfigurationAsync(dhVM).ConfigureAwait(false);

                return await Task.FromResult<UpdateApplicationConfigurationLkViewModelDto>(dipayment);
            }
            catch (System.Exception ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                throw ex;
            }

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<CreateApplicationConfigurationLkViewModelDto>> CreateConfiguration([FromBody]CreateApplicationConfigurationLkViewModelDto dhVM)
        {
            try
            {
                var dipayment = await _service.CreateConfigurationAsync(dhVM).ConfigureAwait(false);

                return await Task.FromResult<CreateApplicationConfigurationLkViewModelDto>(dipayment);
            }
            catch (System.Exception ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                throw ex;
            }

        }

        [HttpPost("[action]")]
        public async Task<ActionResult<DeleteApplicationConfigurationLkViewModelDto>> DeleteConfiguration([FromBody]DeleteApplicationConfigurationLkViewModelDto dhVM)
        {
            try
            {
                var dipayment = await _service.DeleteConfigurationAsync(dhVM).ConfigureAwait(false);

                return await Task.FromResult<DeleteApplicationConfigurationLkViewModelDto>(dipayment);
            }
            catch (System.Exception ex)
            {
                var model = _commonHelper.GetCurrentContollerActionName(this.ControllerContext, ex);
                _commonHelper.LogException(model);
                throw ex;
            }

        }
    }
}
