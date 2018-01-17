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

        /// <summary>
        /// If the given recipe has no existing components in the DB (it's new),
        /// adds the given ingredients to its listing. If it does have existing
        /// ingredients, it's either been previously imported or edited, and we
        /// don't want to potentially overwrite user updates with import data,
        /// so we don't change anything. The additional arguments assure that
        /// we don't perform a million DB hits for dumb lookups
        /// </summary>
        Task ImportIngredients(Guid recipeId, 
                               IEnumerable<ImportIngredient> ingredients,
                               IDictionary<string, int> unitMap,
                               IDictionary<string, Guid> componentMap);
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

        public async Task ImportIngredients(Guid recipeId, 
                                            IEnumerable<ImportIngredient> ingredients,
                                            IDictionary<string, int> unitMap,
                                            IDictionary<string, Guid> componentMap)
        {
            var existingIngredients = await _context.Ingredient.AnyAsync(a => a.RecipeId == recipeId);
            if(existingIngredients)
            {
                return;
            }
            
            var dbIngredients = ingredients.Select(a => new Ingredient
            {
                Id = Guid.NewGuid(),
                RecipeId = recipeId,
                ComponentId = componentMap[a.IngredientName],
                Quantity = a.Amount,
                UnitId = unitMap[a.Unit],
                Number = a.Index
            });

            await _context.Ingredient.AddRangeAsync(dbIngredients);
            await _context.SaveChangesAsync();
        }
    }
}