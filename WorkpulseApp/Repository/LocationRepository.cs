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

namespace CORTNE.Repository
{
    public interface ILocationRepository
    {
        Task<List<CORTNE.ViewModel.LocationViewModel>> GetLocationsAsync();
        Task<LocationViewModel> CreateLocationAsync(LocationViewModel location);
        Task<List<CORTNE.ViewModel.USStatesViewModel>> GetUSStatesAsync();
    }
    public class LocationRepository : ILocationRepository
    {
        private readonly CORTNEDEVContext _context;
        private readonly AppSettings _appSettings;

        public LocationRepository(CORTNEDEVContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public async Task<List<CORTNE.ViewModel.LocationViewModel>> GetLocationsAsync()
        {
            try
            {
                List<LocationViewModel> vm = new List<LocationViewModel>();
                var locations = await _context.LocationLk.ToListAsync().ConfigureAwait(false);
                if (locations != null && locations.Count > 0)
                {
                    foreach (var location in locations)
                    {
                        vm.Add(new LocationViewModel
                        {
                            LocationId = location.LocationId,
                            Code = location.Code,
                            Flaircode = location.Flaircode,
                            Name = location.Name,
                            OrgCode = location.OrgCode,
                            UpdatedBy = location.UpdatedBy,
                            UpdatedDate = location.UpdatedDate
                        });
                    }
                }
                return await Task.FromResult<List<LocationViewModel>>(vm);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in LocationRepository.GetLocationsAsync()", ex);
            }
        }

        public async Task<LocationViewModel> CreateLocationAsync(LocationViewModel location)
        {
            try
            {
                LocationLk loc = new LocationLk();
                loc.LocationId = location.LocationId;
                loc.Code = location.Code;
                loc.Flaircode = location.Flaircode;
                loc.Name = location.Name;
                loc.OrgCode = location.OrgCode;
                loc.UpdatedBy = location.UpdatedBy;
                loc.UpdatedDate = DateTime.Now;

                _context.LocationLk.Add(loc);
                var result = await _context.SaveChangesAsync().ConfigureAwait(false);
                location.LocationId = loc.LocationId;
                
                return await Task.FromResult<LocationViewModel>(location);
            }
            catch (System.Exception ex)
            {
                throw new RepositoryException($"Exception in LocationRepository.CreateLocationAsync({nameof(location)})", ex);
            }
        }

        public async Task<List<CORTNE.ViewModel.USStatesViewModel>> GetUSStatesAsync()
        {
            try
            {
                List<USStatesViewModel> vm = new List<USStatesViewModel>();
                var states = await _context.USStates.OrderBy(a => a.Name).ToListAsync().ConfigureAwait(false);
                if (states != null && states.Count > 0)
                {
                    foreach (var state in states)
                    {
                        vm.Add(new USStatesViewModel
                        {
                            Id = state.Id,
                            Name = state.Name,
                            AlphaCode = state.AlphaCode,
                            FIPSCode = state.FIPSCode
                        });
                    }
                }
                return await Task.FromResult<List<USStatesViewModel>>(vm);
            }
            catch (System.Exception ex)
            {

                throw new RepositoryException($"Exception in LocationRepository.GetUSStatesAsync()", ex);
            }
        }

    }
}
