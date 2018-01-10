using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Take02.Models;
using Take02.ViewModels;

namespace Take02.Controllers
{
    public class RecipesController : Controller
    {
        private readonly CocktailsContext _context;

        public RecipesController(CocktailsContext context)
        {
            _context = context;
        }

        // GET: Recipes
        public async Task<IActionResult> Index(bool showIngredients = false)
        {
            var models = await Helper.GetRecipeViewModelsAsync(_context, showIngredients);
            return View(models);
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || !RecipeExists(id.Value))
            {
                return NotFound();
            }

            var model = await Helper.GetRecipeViewModelAsync(_context, id.Value, true);
            return View(model);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            var model = new RecipeViewModel();
            model.IngredientViewModels.Add(new IngredientViewModel());
            model.LibrarySelectListItems = Helper.GetLibrarySelectListItems(_context);
            model.MixTypeSelectListItems = Helper.GetMixTypeSelectListItems(_context);
            model.ComponentSelectListItems = Helper.GetComponentSelectListItems(_context);
            model.UnitSelectListItems = Helper.GetUnitSelectListItems(_context);
            return View(model);
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,LibraryId,Name,MixTypeId,Instructions,Source")] Recipe recipe,
            RecipeViewModel recipeVM)
        {
            if (ModelState.IsValid)
            {
                recipe.Id = Guid.NewGuid();
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                foreach (var ingredientVM in recipeVM.IngredientViewModels)
                {
                    var ingredient = new Ingredient
                    {
                        Id = Guid.NewGuid(),
                        RecipeId = recipe.Id,
                        ComponentId = ingredientVM.ComponentId,
                        Quantity = ingredientVM.Quantity,
                        UnitId = ingredientVM.UnitId,
                        Number = ingredientVM.Number
                    };
                    _context.Add(ingredient);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !RecipeExists(id.Value))
            {
                return NotFound();
            }

            var model = await Helper.GetRecipeViewModelAsync(_context, id.Value, true);
            model.LibrarySelectListItems = Helper.GetLibrarySelectListItems(_context);
            model.MixTypeSelectListItems = Helper.GetMixTypeSelectListItems(_context);
            model.ComponentSelectListItems = Helper.GetComponentSelectListItems(_context);
            model.UnitSelectListItems = Helper.GetUnitSelectListItems(_context);
            return View(model);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            Guid id,
            [Bind("Id,LibraryId,Name,MixTypeId,Instructions,Source")] Recipe recipe,
            RecipeViewModel recipeVM)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    foreach (var ingredientVM in recipeVM.IngredientViewModels)
                    {
                        bool isNew = false;
                        var ingredient = Helper.GetIngredient(_context, ingredientVM.Id);

                        if (ingredient == null)
                        {
                            isNew = true;
                            ingredient = new Ingredient
                            {
                                Id = ingredientVM.Id
                            };
                        }

                        ingredient.RecipeId = recipe.Id;
                        ingredient.ComponentId = ingredientVM.ComponentId;
                        ingredient.Quantity = ingredientVM.Quantity;
                        ingredient.UnitId = ingredientVM.UnitId;
                        ingredient.Number = ingredientVM.Number;

                        if (isNew)
                        {
                            _context.Add(ingredient);
                        }
                        else
                        {
                            _context.Update(ingredient);
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/AddNewIngredient/5
        public ActionResult AddNewIngredient(Guid? id)
        {
            var model = new IngredientViewModel();

            if (id != null && RecipeExists(id.Value))
            {
                model.RecipeId = id.Value;
                model.ComponentSelectListItems = Helper.GetComponentSelectListItems(_context);
                model.UnitSelectListItems = Helper.GetUnitSelectListItems(_context);
            }

            return PartialView("_AddEditIngredientPartial", model);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || !RecipeExists(id.Value))
            {
                return NotFound();
            }

            var model = await Helper.GetRecipeViewModelAsync(_context, id.Value, true);

            return View(model);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var recipe = await _context.Recipe
                .SingleOrDefaultAsync(m => m.Id == id);
            var ingredients = await Helper.GetIngredientsByRecipeAsync(_context, id);
            ingredients.ForEach(t => _context.Ingredient.Remove(t));
            _context.Recipe.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(Guid id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }
    }
}
