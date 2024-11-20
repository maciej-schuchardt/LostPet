using System.Diagnostics.CodeAnalysis;
using LostPet.Data;
using LostPet.Models;
using Microsoft.AspNetCore.Identity;

namespace LostPet.Services
{
    [ExcludeFromCodeCoverage]
    public class ReportSightingPetViewModel
    {
        public Pet Pet { get; set; }
        public Report Report { get; set; }
        public List<SightingsViewModel> Sightings { get; set; }
        public string ReporterEmail { get; set; }
    }
    
    [ExcludeFromCodeCoverage]
    public class FilterService
    {
        public event Func<Task> UpdateEvent;

        private List<ReportSightingPetViewModel> _view;
        public List<ReportSightingPetViewModel> View
        { 
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                UpdateEvent?.Invoke();
            } }
        UserIdentityProcessor _userIdentityProcessor;
        ReportService reportService;
        SightingsService sightingsService;
        UserManager<ApplicationUser> _userManager;

        [ExcludeFromCodeCoverage]
        public FilterService(UserIdentityProcessor userIdentityProcessor, ReportService reportService, SightingsService sightingsService, UserManager<ApplicationUser> userManager)
        { 
            _userIdentityProcessor = userIdentityProcessor;
            this.reportService = reportService;
            this.sightingsService = sightingsService;
            this._userManager = userManager;
        }

        public async Task Filter(Func<Pet, bool> filter)
        {
            var userId = await this._userIdentityProcessor.GetCurrentUserId();

            var reports = await reportService.GetByFilterAsync(filter);
            var sightings = await sightingsService.GetAllAsync();
            this.View = reports.Select(x => new ReportSightingPetViewModel()
            {
                Pet = x.Pet,
                Report = x.Report,
                Sightings = sightings.Where(y => y.Pet.PetID == x.Pet.PetID).Reverse().ToList(),
                ReporterEmail = _userManager.Users.SingleOrDefault(u => u.Id == x.Report.UserID).Email,
            }).ToList();
        }
    }
}
