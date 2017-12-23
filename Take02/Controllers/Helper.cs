using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take02.Models;
using Take02.ViewModels;

namespace Take02.Controllers
{
    public class Helper
    {
        #region Model Methods

        public static async Task<Component> GetComponentAsync(CocktailsContext _context, Guid componentId)
        {
            var component = await _context.Component
                .SingleOrDefaultAsync(m => m.Id == componentId);
            return component;
        }

        public static Component GetComponent(CocktailsContext _context, Guid componentId)
        {
            var component = _context.Component
                .SingleOrDefault(m => m.Id == componentId);
            return component;
        }

        public static async Task<List<Component>> GetComponentsAsync(CocktailsContext _context)
        {
            var components = await _context.Component
                .ToListAsync();
            return components;
        }

        public static List<Component> GetComponents(CocktailsContext _context)
        {
            return GetComponentsAsync(_context).Result;
        }

        public static async Task<ComponentType> GetComponentTypeAsync(CocktailsContext _context, int componentTypeId)
        {
            var componentType = await _context.ComponentType
                .SingleOrDefaultAsync(m => m.Id == componentTypeId);
            return componentType;
        }

        public static ComponentType GetComponentType(CocktailsContext _context, int componentTypeId)
        {
            var componentType = _context.ComponentType
                .SingleOrDefault(m => m.Id == componentTypeId);
            return componentType;
        }

        public static async Task<List<ComponentType>> GetComponentTypesAsync(CocktailsContext _context)
        {
            var componentTypes = await _context.ComponentType
                .ToListAsync();
            return componentTypes;
        }

        public static List<ComponentType> GetComponentTypes(CocktailsContext _context)
        {
            return GetComponentTypesAsync(_context).Result;
        }

        public static async Task<Ingredient> GetIngredientAsync(CocktailsContext _context, Guid ingredientId)
        {
            var ingredient = await _context.Ingredient
                .SingleOrDefaultAsync(m => m.Id == ingredientId);
            return ingredient;
        }

        public static Ingredient GetIngredient(CocktailsContext _context, Guid ingredientId)
        {
            var ingredient = _context.Ingredient
                .SingleOrDefault(m => m.Id == ingredientId);
            return ingredient;
        }

        public static async Task<List<Ingredient>> GetIngredientsAsync(CocktailsContext _context)
        {
            var ingredients = await _context.Ingredient
                .ToListAsync();
            return ingredients;
        }

        public static List<Ingredient> GetIngredients(CocktailsContext _context)
        {
            var ingredients = _context.Ingredient
                .ToList();
            return ingredients;
        }

        public static async Task<Library> GetLibraryAsync(CocktailsContext _context, Guid libraryId)
        {
            var library = await _context.Library
                .SingleOrDefaultAsync(m => m.Id == libraryId);
            return library;
        }

        public static Library GetLibrary(CocktailsContext _context, Guid libraryId)
        {
            var library = _context.Library
                .SingleOrDefault(m => m.Id == libraryId);
            return library;
        }

        public static async Task<Recipe> GetRecipeAsync(CocktailsContext _context, Guid recipeId)
        {
            var recipe = await _context.Recipe
                .SingleOrDefaultAsync(m => m.Id == recipeId);
            return recipe;
        }

        public static Recipe GetRecipe(CocktailsContext _context, Guid recipeId)
        {
            var recipe = _context.Recipe
                .SingleOrDefault(m => m.Id == recipeId);
            return recipe;
        }

        public static async Task<List<Recipe>> GetRecipesAsync(CocktailsContext _context)
        {
            var recipes = await _context.Recipe
                .ToListAsync();
            return recipes;
        }

        public static List<Recipe> GetRecipes(CocktailsContext _context)
        {
            return GetRecipesAsync(_context).Result;
        }

        public static async Task<Unit> GetUnitAsync(CocktailsContext _context, int unitId)
        {
            var unit = await _context.Unit
                .SingleOrDefaultAsync(m => m.Id == unitId);
            return unit;
        }

        public static Unit GetUnit(CocktailsContext _context, int unitId)
        {
            var unit = _context.Unit
                .SingleOrDefault(m => m.Id == unitId);
            return unit;
        }

        public static async Task<List<Unit>> GetUnitsAsync(CocktailsContext _context)
        {
            var units = await _context.Unit
                .ToListAsync();
            return units;
        }

        public static List<Unit> GetUnits(CocktailsContext _context)
        {
            return GetUnitsAsync(_context).Result;
        }

        #endregion Model Methods

        #region ViewModel Methods

        public static async Task<IngredientViewModel> GetIngredientViewModelAsync(CocktailsContext _context, Guid id)
        {
            var ingredient = await GetIngredientAsync(_context, id);

            var recipe = await GetRecipeAsync(_context, ingredient.RecipeId);

            var component = await GetComponentAsync(_context, ingredient.ComponentId);

            var unit = await GetUnitAsync(_context, ingredient.UnitId);

            var ingredientViewModel = new IngredientViewModel(ingredient, recipe, component, unit);

            return ingredientViewModel;
        }

        public static IngredientViewModel GetIngredientViewModel(CocktailsContext _context, Guid id)
        {
            var ingredient = GetIngredient(_context, id);

            var recipe = GetRecipe(_context, ingredient.RecipeId);

            var component = GetComponent(_context, ingredient.ComponentId);

            var unit = GetUnit(_context, ingredient.UnitId);

            var ingredientViewModel = new IngredientViewModel(ingredient, recipe, component, unit);

            return ingredientViewModel;
        }

        public static async Task<List<IngredientViewModel>> GetIngredientViewModelsAsync(CocktailsContext _context)
        {
            var ingredients = await GetIngredientsAsync(_context);

            var ingredientViewModels = ingredients.Select(t => GetIngredientViewModelAsync(_context, t.Id).Result).ToList();

            return ingredientViewModels;
        }

        public static List<IngredientViewModel> GetIngredientViewModels(CocktailsContext _context)
        {
            var ingredients = GetIngredients(_context);

            var ingredientViewModels = ingredients.Select(t => GetIngredientViewModel(_context, t.Id)).ToList();

            return ingredientViewModels;
        }

        #endregion ViewModel Methods

        #region SelectListItems Methods

        public static List<SelectListItem> GetComponentSelectListItems(CocktailsContext _context)
        {
            var components = GetComponents(_context);

            var items = components.ConvertAll(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });

            return items;
        }

        public static List<SelectListItem> GetComponentTypeSelectListItems(CocktailsContext _context)
        {
            var componentTypes = GetComponentTypes(_context);

            var items = componentTypes.ConvertAll(t => new SelectListItem()
            {
                Text = t.Type,
                Value = t.Id.ToString()
            });

            return items;
        }

        public static List<SelectListItem> GetRecipeSelectListItems(CocktailsContext _context)
        {
            var recipes = GetRecipes(_context);

            var items = recipes.ConvertAll(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });

            return items;
        }

        public static List<SelectListItem> GetUnitSelectListItems(CocktailsContext _context)
        {
            var units = GetUnits(_context);

            var items = units.ConvertAll(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });

            return items;
        }

        #endregion SelectListItemMethods
    }
}
