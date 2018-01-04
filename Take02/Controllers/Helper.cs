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
            return GetComponentAsync(_context, componentId).Result;
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
            return GetComponentTypeAsync(_context, componentTypeId).Result;
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
            return GetIngredientAsync(_context, ingredientId).Result;
        }

        public static async Task<List<Ingredient>> GetIngredientsAsync(CocktailsContext _context)
        {
            var ingredients = await _context.Ingredient
                .ToListAsync();
            return ingredients;
        }

        public static List<Ingredient> GetIngredients(CocktailsContext _context)
        {
            return GetIngredientsAsync(_context).Result;
        }

        public static async Task<List<Ingredient>> GetIngredientsByRecipeAsync(CocktailsContext _context, Guid recipeId)
        {
            var recipe = GetRecipe(_context, recipeId);

            var ingredients = await _context.Ingredient
                .Where(t => t.RecipeId == recipeId)
                .OrderBy(t => t.Number)
                .ToListAsync();

            return ingredients;
        }

        public static List<Ingredient> GetIngredientsByRecipe(CocktailsContext _context, Guid recipeId)
        {
            return GetIngredientsByRecipeAsync(_context, recipeId).Result;
        }

        public static async Task<Library> GetLibraryAsync(CocktailsContext _context, Guid libraryId)
        {
            var library = await _context.Library
                .SingleOrDefaultAsync(m => m.Id == libraryId);
            return library;
        }

        public static Library GetLibrary(CocktailsContext _context, Guid libraryId)
        {
            return GetLibraryAsync(_context, libraryId).Result;
        }

        public static async Task<List<Library>> GetLibrariessAsync(CocktailsContext _context)
        {
            var libraries = await _context.Library
                .ToListAsync();
            return libraries;
        }

        public static List<Library> GetLibraries(CocktailsContext _context)
        {
            return GetLibrariessAsync(_context).Result;
        }

        public static async Task<MixType> GetMixTypeAsync(CocktailsContext _context, int mixTypeId)
        {
            var mixType = await _context.MixType
                .SingleOrDefaultAsync(mbox => mbox.Id == mixTypeId);
            return mixType;
        }

        public static MixType GetMixType(CocktailsContext _context, int mixTypeId)
        {
            return GetMixTypeAsync(_context, mixTypeId).Result;
        }

        public static async Task<List<MixType>> GetMixTypesAsync(CocktailsContext _context)
        {
            var mixTypes = await _context.MixType
                .ToListAsync();
            return mixTypes;
        }

        public static List<MixType> GetMixTypes(CocktailsContext _context)
        {
            return GetMixTypesAsync(_context).Result;
        }

        public static async Task<Recipe> GetRecipeAsync(CocktailsContext _context, Guid recipeId)
        {
            var recipe = await _context.Recipe
                .SingleOrDefaultAsync(m => m.Id == recipeId);
            return recipe;
        }

        public static Recipe GetRecipe(CocktailsContext _context, Guid recipeId)
        {
            return GetRecipeAsync(_context, recipeId).Result;
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
            return GetUnitAsync(_context, unitId).Result;
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

        public static async Task<ComponentViewModel> GetComponentViewModelAsync(CocktailsContext _context, Guid id)
        {
            var component = await GetComponentAsync(_context, id);

            var componentType = await GetComponentTypeAsync(_context, component.ComponentTypeId);

            var model = new ComponentViewModel(component, componentType);

            return model;
        }

        public static async Task<List<ComponentViewModel>> GetComponentViewModelsAsync(CocktailsContext _context)
        {
            var components = await GetComponentsAsync(_context);

            var models = components.Select(t => GetComponentViewModelAsync(_context, t.Id).Result).ToList();

            return models;
        }

        public static async Task<IngredientViewModel> GetIngredientViewModelAsync(CocktailsContext _context, Guid id)
        {
            var ingredient = await GetIngredientAsync(_context, id);

            var recipe = await GetRecipeAsync(_context, ingredient.RecipeId);

            var component = await GetComponentAsync(_context, ingredient.ComponentId);

            var unit = await GetUnitAsync(_context, ingredient.UnitId);

            var model = new IngredientViewModel(ingredient, recipe, component, unit);

            return model;
        }

        public static async Task<List<IngredientViewModel>> GetIngredientViewModelsAsync(CocktailsContext _context)
        {
            var ingredients = await GetIngredientsAsync(_context);

            var models = ingredients.Select(t => GetIngredientViewModelAsync(_context, t.Id).Result).ToList();

            return models;
        }

        public static async Task<List<IngredientViewModel>> GetIngredientViewModelsByRecipeAsync(CocktailsContext _context, Guid recipeId)
        {
            var recipe = await GetRecipeAsync(_context, recipeId);

            var ingredients = await GetIngredientsByRecipeAsync(_context, recipe.Id);

            var viewModels = new List<IngredientViewModel>();

            foreach (var ingredient in ingredients)
            {
                var component = await GetComponentAsync(_context, ingredient.ComponentId);

                var unit = await GetUnitAsync(_context, ingredient.UnitId);

                viewModels.Add(new IngredientViewModel(ingredient, recipe, component, unit));
            }

            return viewModels;
        }

        public static async Task<RecipeViewModel> GetRecipeViewModelAsync(CocktailsContext _context, Guid id, bool includeIngredients = false)
        {
            var recipe = await GetRecipeAsync(_context, id);

            var library = await GetLibraryAsync(_context, recipe.LibraryId);

            var mixType = await GetMixTypeAsync(_context, recipe.MixTypeId);

            var model = new RecipeViewModel(recipe, library, mixType);

            if (includeIngredients)
            {
                model.IngredientViewModels = await GetIngredientViewModelsByRecipeAsync(_context, id);
            }

            return model;
        }

        public static async Task<List<RecipeViewModel>> GetRecipeViewModelsAsync(CocktailsContext _context, bool includeIngredients = false)
        {
            var recipes = await GetRecipesAsync(_context);

            var models = recipes.Select(t => GetRecipeViewModelAsync(_context, t.Id).Result).ToList();

            if (includeIngredients)
            {
                foreach (var model in models)
                {
                    model.IngredientViewModels = await GetIngredientViewModelsByRecipeAsync(_context, model.Id);
                    model.ShowIngredients = includeIngredients;
                }
            }

            return models;
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
                Text = t.Name,
                Value = t.Id.ToString()
            });

            return items;
        }

        public static List<SelectListItem> GetIngredientSelectListItems(CocktailsContext _context)
        {
            var ingredients = GetIngredients(_context);

            var items = new List<SelectListItem>();

            foreach (var ingredient in ingredients)
            {
                var component = GetComponent(_context, ingredient.ComponentId);

                items.Add(new SelectListItem()
                {
                    Text = ingredient.Number.ToString() + " " + component.Name,
                    Value = ingredient.Number.ToString()
                });
            }

            return items;
        }

        public static List<SelectListItem> GetLibrarySelectListItems(CocktailsContext _context)
        {
            var libraries = GetLibraries(_context);

            var items = libraries.ConvertAll(t => new SelectListItem()
            {
                Text = t.Name,
                Value = t.Id.ToString()
            });

            return items;
        }

        public static List<SelectListItem> GetMixTypeSelectListItems(CocktailsContext _context)
        {
            var mixTypes = GetMixTypes(_context);

            var items = mixTypes.ConvertAll(t => new SelectListItem()
            {
                Text = t.Name,
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
