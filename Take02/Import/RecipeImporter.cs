using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Take02.Models;

namespace Take02.Import
{
    public interface IRecipeImporter
    {
        /// <summary>
        /// An idempotent operation which takes in a Library ID and
        /// recipe, and either returns the ID for the existing Recipe
        /// or creates a new one and returns the new ID
        /// </summary>
        Task<Guid> ImportRecipe(Guid libraryId, ImportRecipe recipe);
    }

    public class RecipeImporter : IRecipeImporter
    {
        private readonly CocktailsContext _context;

        public RecipeImporter(CocktailsContext context)
        {
            _context = context;
        }

        public async Task<Guid> ImportRecipe(Guid libraryId, ImportRecipe recipe)
        {
            var dbRecipe = await _context
            .Recipe
            .FirstOrDefaultAsync(a => a.LibraryId == libraryId && a.Name == recipe.RecipeName);
            
            if(dbRecipe != null)
            {
                return dbRecipe.Id;
            }

            dbRecipe = new Recipe
            {
                Id = Guid.NewGuid(),
                LibraryId = libraryId,
                Name = recipe.RecipeName,
                MixTypeId = recipe.MixMethod,
                Instructions = recipe.Instructions,
                Source = "Automated import"
            };

            await _context.Recipe.AddAsync(dbRecipe);
            await _context.SaveChangesAsync();

            return dbRecipe.Id;
        }
    }
}