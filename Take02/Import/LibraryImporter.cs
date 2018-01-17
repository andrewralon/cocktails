using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Take02.Models;

namespace Take02.Import
{
    public interface ILibraryImporter
    {
        /// <summary>
        /// An idempotent operation which takes in a set of Library names
        /// and returns a mapping for the Library names to their database IDs
        /// </summary>
        Task<IDictionary<string, Guid>> ImportLibraries(IEnumerable<string> libraries);
    }

    public class LibraryImporter : ILibraryImporter
    {
        private readonly CocktailsContext _context;

        public LibraryImporter(CocktailsContext context)
        {
            _context = context;
        }

        public async Task<IDictionary<string, Guid>> ImportLibraries(IEnumerable<string> libraries)
        {
            var uniqueNames = libraries.Distinct();
            var response = new Dictionary<string, Guid>();

            // Assumes a small number of libraries in the set
            foreach(var libraryName in libraries)
            {
                var dbLibrary = await _context
                .Library
                .FirstOrDefaultAsync(a => a.Name == libraryName);

                if(dbLibrary != null)
                {
                    response[libraryName] = dbLibrary.Id;
                    continue;
                }

                dbLibrary = new Library
                {
                    Id = Guid.NewGuid(),
                    Name = libraryName
                };
                await _context.Library.AddAsync(dbLibrary);
                response[libraryName] = dbLibrary.Id;
            }

            await _context.SaveChangesAsync();
            return response;
        }
    }
}