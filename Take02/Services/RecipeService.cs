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
    public interface IRecipeService
    {
        Task<ICollection<Recipe>> GetAllRecipesAsync();
        Task<ICollection<MixType>> GetAllMixTypesAsync();

        // Once this application scales past thousands of recipes, we'll
        // have to adjust callers to this method and batch it somehow.
        // However, we can just pull them all while the dataset is relatively
        // small.
        Task<ICollection<Ingredient>> GetAllIngredientsAsync();

        Task<ICollection<Component>> GetAllComponentsAsync();
        Task<ICollection<Unit>> GetAllUnitsAsync();

        Task<ICollection<Recipe>> GetAllRecipesByLibraryAsync(Guid libraryId);
    }

    public class RecipeService : IRecipeService
    {
        private readonly CocktailsContext _db;

        public RecipeService(CocktailsContext db)
        {
            _db = db;
        }

        public async Task<ICollection<Recipe>> GetAllRecipesAsync()
        {
            return (await _db
            .Recipe
            .OrderBy(t => t.Name)
            .ToListAsync());
        }

        public async Task<ICollection<MixType>> GetAllMixTypesAsync()
        {
            return (await _db.MixType.ToListAsync());
        }

        public async Task<ICollection<Ingredient>> GetAllIngredientsAsync()
        {
            return (await _db.Ingredient.ToListAsync());
        }

        public async Task<ICollection<Component>> GetAllComponentsAsync()
        {
            return (await _db.Component.ToListAsync());
        }

        public async Task<ICollection<Unit>> GetAllUnitsAsync()
        {
            return (await _db.Unit.ToListAsync());
        }

        public async Task<ICollection<Recipe>> GetAllRecipesByLibraryAsync(Guid libraryId)
        {
            return (await _db
                .Recipe
                .Where(t => t.LibraryId == libraryId)
                .OrderBy(t => t.Name)
                .ToListAsync());
        }
    }
}
