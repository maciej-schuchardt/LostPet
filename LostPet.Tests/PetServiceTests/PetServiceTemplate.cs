using System.Diagnostics.CodeAnalysis;
using LostPet.Models;
using LostPet.Services;
using LostPet.Tests.Templates;
using Microsoft.EntityFrameworkCore;

namespace LostPet.Tests.PetServiceTests;

[ExcludeFromCodeCoverage]
public abstract class PetServiceTemplate : CommonEvents<PetService>
{    
    private PetService service;

    protected override PetService Service { get => service; }

    public override void Setup()
    {
        base.Setup();
        service = new PetService(context);
    }
}
