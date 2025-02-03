using System.Diagnostics.CodeAnalysis;
using LostPet.Services;
using LostPet.Tests.Templates;

namespace LostPet.Tests.ReportServiceTests
{
    [ExcludeFromCodeCoverage]
    public abstract class ReportServiceTemplate : CommonEvents<ReportService>
    {
        private ReportService service;
        protected override ReportService Service { get => service; }

        public override void OneTimeSetUp()
        {
            base.OneTimeSetUp();
            service = new ReportService(context);
        }
    }
}