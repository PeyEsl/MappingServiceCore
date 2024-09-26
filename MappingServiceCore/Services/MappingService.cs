using Microsoft.EntityFrameworkCore;
using MappingServiceCore.Services.Interfaces;
using MappingServiceCore.Models.Entities;
using MappingServiceCore.Data;

namespace MappingServiceCore.Services
{
    public class MappingService : IMappingService
    {
        #region Ctor

        private readonly ApplicationDbContext _context;

        public MappingService(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        public async Task<bool> AddPersonAsync(Person person)
        {
            try
            {
                await _context.Set<Person>()
                              .AddAsync(person);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding person.", ex);
            }
        }

        public async Task<bool> DeletePersonAsync(string id)
        {
            try
            {
                var person = await GetPersonByIdAsync(id);
                if (person == null)
                    return false;

                _context.Set<Person>()
                        .Remove(person);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error soft deleting person.", ex);
            }
        }

        public async Task<bool> ExistsPersonAsync(string id)
        {
            try
            {
                return await _context.Set<Person>()
                                     .AsNoTracking()
                                     .AnyAsync(p => p.Id.ToString() == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error existing person.", ex);
            }
        }

        public async Task<bool> ExistsPersonByNameAsync(string isExists)
        {
            try
            {
                return await _context.Set<Person>()
                                     .AsNoTracking()
                                     .AnyAsync(p => p.LastName == isExists ||
                                                    p.FirstName == isExists ||
                                                    p.PhoneNumber == isExists);
            }
            catch (Exception ex)
            {
                throw new Exception("Error existing person.", ex);
            }
        }

        public async Task<IEnumerable<Person>> GetAllPersonsAsync()
        {
            try
            {
                return await _context.Set<Person>()
                                     .AsNoTracking()
                                     .OrderBy(p => p.LastName)
                                     .OrderBy(p => p.FirstName)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving people.", ex);
            }
        }

        public async Task<Person?> GetPersonByIdAsync(string id)
        {
            try
            {
                return await _context.Set<Person>()
                                     .FirstOrDefaultAsync(p => p.Id.ToString() == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving person.", ex);
            }
        }

        public async Task<IEnumerable<Person>> SearchPersonsAsync(string searchTerm)
        {
            try
            {
                return await _context.Set<Person>()
                                     .AsNoTracking()
                                     .Where(p => p.FirstName!.Contains(searchTerm) ||
                                                 p.LastName!.Contains(searchTerm) ||
                                                 p.PhoneNumber!.Contains(searchTerm))
                                     .OrderBy(p => p.LastName)
                                     .OrderBy(p => p.FirstName)
                                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error searching people with term: {searchTerm}.", ex);
            }
        }

        public async Task<bool> UpdatePersonAsync(Person person)
        {
            try
            {
                var existingPerson = await _context.Set<Person>()
                                                   .FirstOrDefaultAsync(c => c.Id == person.Id);
                if (existingPerson == null)
                    throw new Exception("Person not found.");

                existingPerson.FirstName = person.FirstName;
                existingPerson.LastName = person.LastName;
                existingPerson.PhoneNumber = person.PhoneNumber;

                _context.Set<Person>()
                        .Update(existingPerson);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating person.", ex);
            }
        }
    }
}
