using CORTNE.EFModelCORTNEDB;
using CORTNE.Exception;
using CORTNE.Helpers;
using CORTNE.Models;
using CORTNE.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CORTNE.Repository.ApplicationConfiguration
{
    public class ApplicationConfigurationRepository : IApplicationConfigurationRepository
    {
        private readonly CORTNEDEVContext _context;
        private readonly AppSettings _appSettings;

        public ApplicationConfigurationRepository(CORTNEDEVContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<List<GetApplicationConfigurationLkViewModelDto>> GetDebitMemoConfigurationsAsync()
        {
            try
            {
                var lst = await _context.ApplicationConfigurationLk.Where(a => a.Area == "DM" && a.IsDeleted == false).OrderBy(a => a.Description).ToListAsync().ConfigureAwait(false);
                var conList = new List<GetApplicationConfigurationLkViewModelDto>();

                foreach (var con in lst)
                {
                    var conItem = new GetApplicationConfigurationLkViewModelDto();
                    conItem.ID = con.ID;
                    conItem.Description = con.Description;
                    conItem.Value = con.Value;
                    conList.Add(conItem);
                }

                return await Task.FromResult<List<GetApplicationConfigurationLkViewModelDto>>(conList);
            }
            catch (System.Exception ex)
            {
                throw new RepositoryException($"Exception in ApplicationConfigurationRepository.GetDebitMemoConfigurationsAsync()", ex);
            }
        }

        public async Task<List<GetApplicationConfigurationLkViewModelDto>> GetAOBConfigurationsAsync()
        {
            try
            {
                var lst = await _context.ApplicationConfigurationLk.Where(a => a.Area == "AOB" && a.IsDeleted == false).OrderBy(a => a.Description).ToListAsync().ConfigureAwait(false);
                var conList = new List<GetApplicationConfigurationLkViewModelDto>();

                foreach (var con in lst)
                {
                    var conItem = new GetApplicationConfigurationLkViewModelDto();
                    conItem.ID = con.ID;
                    conItem.Description = con.Description;
                    conItem.Value = con.Value;
                    conList.Add(conItem);
                }

                return await Task.FromResult<List<GetApplicationConfigurationLkViewModelDto>>(conList);
            }
            catch (System.Exception ex)
            {
                throw new RepositoryException($"Exception in ApplicationConfigurationRepository.GetAOBConfigurationsAsync()", ex);
            }
        }

        public async Task<List<GetApplicationConfigurationLkViewModelDto>> GetMFCConfigurationsAsync()
        {
            try
            {
                var lst = await _context.ApplicationConfigurationLk.Where(a => a.Area == "MFC" && a.IsDeleted == false).OrderBy(a => a.Description).ToListAsync().ConfigureAwait(false);
                var conList = new List<GetApplicationConfigurationLkViewModelDto>();

                foreach (var con in lst)
                {
                    var conItem = new GetApplicationConfigurationLkViewModelDto();
                    conItem.ID = con.ID;
                    conItem.Description = con.Description;
                    conItem.Value = con.Value;
                    conList.Add(conItem);
                }

                return await Task.FromResult<List<GetApplicationConfigurationLkViewModelDto>>(conList);
            }
            catch (System.Exception ex)
            {
                throw new RepositoryException($"Exception in ApplicationConfigurationRepository.GetMFCConfigurationsAsync()", ex);
            }
        }

        public async Task<UpdateApplicationConfigurationLkViewModelDto> UpdateConfigurationAsync(UpdateApplicationConfigurationLkViewModelDto dhVM)
        {
            try
            {
                var conUp = await _context.ApplicationConfigurationLk.Where(a => a.ID == dhVM.ID).FirstOrDefaultAsync().ConfigureAwait(false);

                if (conUp != null)
                {
                    conUp.Description = dhVM.Description;
                    conUp.Value = dhVM.Value;
                    conUp.UpdatedBy = dhVM.UpdatedBy;
                    conUp.UpdatedDate = DateTime.UtcNow;

                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }

                return await Task.FromResult<UpdateApplicationConfigurationLkViewModelDto>(dhVM);

            }
            catch (System.Exception ex)
            {
                throw new RepositoryException($"Exception in ApplicationConfigurationRepository.UpdateConfigurationAsync({nameof(dhVM.ID)})", ex);
            }

        }


        public async Task<CreateApplicationConfigurationLkViewModelDto> CreateConfigurationAsync(CreateApplicationConfigurationLkViewModelDto dhVM)
        {
            try
            {
                var con = new ApplicationConfigurationLk();                
                con.Area = dhVM.Area.Trim();
                con.Description = dhVM.Description;
                con.Value = dhVM.Value;
                con.UpdatedBy = dhVM.UpdatedBy;
                con.UpdatedDate = DateTime.UtcNow;
                con.IsDeleted = false;

                _context.ApplicationConfigurationLk.Add(con);

                var result = await _context.SaveChangesAsync().ConfigureAwait(false);

                return await Task.FromResult<CreateApplicationConfigurationLkViewModelDto>(dhVM);

            }
            catch (System.Exception ex)
            {
                throw new RepositoryException($"Exception in ApplicationConfigurationRepository.CreateConfigurationAsync()", ex);
            }
        }

        public async Task<DeleteApplicationConfigurationLkViewModelDto> DeleteConfigurationAsync(DeleteApplicationConfigurationLkViewModelDto dhVM)
        {
            try
            {
                var conUp = await _context.ApplicationConfigurationLk.Where(a => a.ID == dhVM.ID).FirstOrDefaultAsync().ConfigureAwait(false);

                if (conUp != null)
                {
                    conUp.IsDeleted = true;
                    conUp.UpdatedBy = dhVM.UpdatedBy;
                    conUp.UpdatedDate = DateTime.UtcNow;

                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }

                return await Task.FromResult<DeleteApplicationConfigurationLkViewModelDto>(dhVM);

            }
            catch (System.Exception ex)
            {
                throw new RepositoryException($"Exception in ApplicationConfigurationRepository.DeleteConfigurationAsync({nameof(dhVM.ID)})", ex);
            }
        }
    }
}
