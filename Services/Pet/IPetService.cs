namespace LostPet.Services.Pet
{
    public interface IPetService
    {
        Task<List<Models.Pet>> GetAllPets();
    }
}
