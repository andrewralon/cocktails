using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take02.Models;
using Take02.ViewModels;

namespace Take02.Services
{
    public interface ILibraryService
    {
        Task<ICollection<Library>> GetAllLibrariesAsync();
        Task<Library> GetLibraryAsync(Guid id);
    }

    public class LibraryService : ILibraryService
    {
        private readonly CocktailsContext _db;

        public LibraryService(CocktailsContext db)
        {
            _db = db;
        }

        public async Task<ICollection<Library>> GetAllLibrariesAsync()
        {
            return (await _db.Library.ToListAsync());
        }

        public async Task<Library> GetLibraryAsync(Guid id)
        {
            return (await _db
            .Library
            .FirstOrDefaultAsync(a => a.Id == id));
        }
    }
}
