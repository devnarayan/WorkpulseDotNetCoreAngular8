using WorkpulseApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkpulseApp.Service.ApplicationConfiguration
{
    public interface IApplicationConfigurationService
    {
        Task<List<GetApplicationConfigurationLkViewModelDto>> GetDebitMemoConfigurationsAsync();

        Task<List<GetApplicationConfigurationLkViewModelDto>> GetAOBConfigurationsAsync();

        Task<List<GetApplicationConfigurationLkViewModelDto>> GetMFCConfigurationsAsync();

        Task<UpdateApplicationConfigurationLkViewModelDto> UpdateConfigurationAsync(UpdateApplicationConfigurationLkViewModelDto dhVM);

        Task<CreateApplicationConfigurationLkViewModelDto> CreateConfigurationAsync(CreateApplicationConfigurationLkViewModelDto dhVM);

        Task<DeleteApplicationConfigurationLkViewModelDto> DeleteConfigurationAsync(DeleteApplicationConfigurationLkViewModelDto dhVM);
    }
}
