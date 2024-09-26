using MappingServiceCore.Models.Entities;

namespace MappingServiceCore.Services.Interfaces
{
    public interface IMappingService
    {
        Task<bool> AddPersonAsync(Person person);
        Task<bool> DeletePersonAsync(string id);
        Task<bool> ExistsPersonAsync(string id);
        Task<bool> ExistsPersonByNameAsync(string name);
        Task<IEnumerable<Person>> GetAllPersonsAsync();
        Task<Person?> GetPersonByIdAsync(string id);
        Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm);
        Task<bool> UpdatePersonAsync(Person person);
    }
}
