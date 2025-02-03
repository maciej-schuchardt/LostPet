using System.Diagnostics.CodeAnalysis;
using LostPet.Services;
using LostPet.Tests.Templates;

namespace LostPet.Tests.SightingsServiceTests;

[ExcludeFromCodeCoverage]
public abstract class SightingsServiceTestsTemplate : CommonEvents<SightingsService>
{    
    private SightingsService service;

    protected override SightingsService Service { get => service; }

    public override void OneTimeSetUp()
    {
        base.OneTimeSetUp();
        service = new SightingsService(context);
    }
}
