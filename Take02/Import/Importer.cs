using System;
using System.Threading.Tasks;
using Take02.Models;

namespace Take02.Import
{
    public interface IImporter
    {
        Task Import(string data);
    }

    public class Importer : IImporter
    {
        private readonly CocktailsContext _context;

        public Importer(CocktailsContext context)
        {
            _context = context;
        }

        public async Task Import(string data)
        {
            throw new Exception();
        }
    }
}