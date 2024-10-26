using LostPet.Data;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace LostPet.Services.Pet
{
    public class PetService(ApplicationDbContext context) : IPetService
    {
        public async Task<List<Models.Pet>> GetAllPets()
        {
            var pets = await context.Pets.ToListAsync();
            pets.ForEach(async p => {
                p.Photo = await ImgToBytes();
            });
            return pets;
        }

        public async Task<string> ImgToBytes()
        {
            //var data = await new HttpClient().GetByteArrayAsync("C:\\Users\\macias\\Documents\\LostPetFTP\\pic.jpg");
            var file = System.IO.File.ReadAllBytes("C:\\Users\\macias\\Documents\\LostPetFTP\\pic.jpg");

            var pdfContent = "data:image;base64, ";
            pdfContent += System.Convert.ToBase64String(file);

            return pdfContent;
        }
    }
}
