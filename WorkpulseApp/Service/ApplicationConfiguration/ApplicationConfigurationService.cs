using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CORTNE.Repository.ApplicationConfiguration;
using CORTNE.ViewModel;

namespace CORTNE.Service.ApplicationConfiguration
{
    public class ApplicationConfigurationService: IApplicationConfigurationService
    {
        private readonly IApplicationConfigurationRepository _applicationConfigurationRepository;

        protected IApplicationConfigurationRepository ApplicationConfigurationRepository { get { return _applicationConfigurationRepository; } }

        public ApplicationConfigurationService(IApplicationConfigurationRepository theapplicationConfigurationRepository)
        {
            _applicationConfigurationRepository = theapplicationConfigurationRepository;
        }

        public async Task<List<GetApplicationConfigurationLkViewModelDto>> GetDebitMemoConfigurationsAsync()
        {
            var task = await _applicationConfigurationRepository.GetDebitMemoConfigurationsAsync();

            if (task == null) return null;

            return task;
        }

        public async Task<List<GetApplicationConfigurationLkViewModelDto>> GetAOBConfigurationsAsync()
        {
            var task = await _applicationConfigurationRepository.GetAOBConfigurationsAsync();

            if (task == null) return null;

            return task;
        }

        public async Task<List<GetApplicationConfigurationLkViewModelDto>> GetMFCConfigurationsAsync()
        {
            var task = await _applicationConfigurationRepository.GetMFCConfigurationsAsync();

            if (task == null) return null;

            return task;
        }

        public async Task<UpdateApplicationConfigurationLkViewModelDto> UpdateConfigurationAsync(UpdateApplicationConfigurationLkViewModelDto dhVM)
        {
            return await _applicationConfigurationRepository.UpdateConfigurationAsync(dhVM);
        }

        public async Task<CreateApplicationConfigurationLkViewModelDto> CreateConfigurationAsync(CreateApplicationConfigurationLkViewModelDto dhVM)
        {
            return await _applicationConfigurationRepository.CreateConfigurationAsync(dhVM);
        }

        public async Task<DeleteApplicationConfigurationLkViewModelDto> DeleteConfigurationAsync(DeleteApplicationConfigurationLkViewModelDto dhVM)
        {
            return await _applicationConfigurationRepository.DeleteConfigurationAsync(dhVM);
        }
    }
}
