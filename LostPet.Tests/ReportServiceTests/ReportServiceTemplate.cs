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

        public override void Setup()
        {
            base.Setup();
            service = new ReportService(context);
        }
    }
}