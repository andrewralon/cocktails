using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Take02.Models;

namespace Take02.Import
{
    public interface IIngredientImporter
    {
        /// <summary>
        /// Takes in an ingredient, sets up any upstream dependency entity,
        /// and inserts the Ingredient into the database
        /// </summary>
        Task ImportIngredient(Guid recipeId, ImportIngredient recipe);
    }

    public class IngredientImporter : IIngredientImporter
    {
        private readonly CocktailsContext _context;

        public IngredientImporter(CocktailsContext context)
        {
            _context = context;
        }

        public async Task ImportIngredient(Guid recipeId, ImportIngredient recipe)
        {
        }
    }
}