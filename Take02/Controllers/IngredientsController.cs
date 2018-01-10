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
    public class IngredientsController : Controller
    {
        private readonly CocktailsContext _context;

        public IngredientsController(CocktailsContext context)
        {
            _context = context;
        }

        // GET: Ingredients
        public async Task<IActionResult> Index()
        {
            var models = await Helper.GetIngredientViewModelsAsync(_context);
            return View(models);
        }

        // GET: Ingredients/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await Helper.GetIngredientViewModelAsync(_context, id.Value);
            return View(model);
        }

        // GET: Ingredients/Create
        public IActionResult Create()
        {
            var model = new IngredientViewModel
            {
                RecipeSelectListItems = Helper.GetRecipeSelectListItems(_context),
                ComponentSelectListItems = Helper.GetComponentSelectListItems(_context),
                UnitSelectListItems = Helper.GetUnitSelectListItems(_context)
            };
            return View(model);
        }

        // POST: Ingredients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,RecipeId,ComponentId,Quantity,UnitId,Number")] Ingredient ingredient)
        {
            if (ModelState.IsValid)
            {
                ingredient.Id = Guid.NewGuid();
                _context.Add(ingredient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ingredient);
        }

        // GET: Ingredients/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await Helper.GetIngredientViewModelAsync(_context, id.Value);
            model.RecipeSelectListItems = Helper.GetRecipeSelectListItems(_context);
            model.ComponentSelectListItems = Helper.GetComponentSelectListItems(_context);
            model.UnitSelectListItems = Helper.GetUnitSelectListItems(_context);
            return View(model);
        }

        // POST: Ingredients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,RecipeId,ComponentId,Quantity,UnitId,Number")] Ingredient ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingredient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngredientExists(ingredient.Id))
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
            return View(ingredient);
        }

        // GET: Ingredients/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await Helper.GetIngredientViewModelAsync(_context, id.Value);
            return View(model);
        }

        // POST: Ingredients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var ingredient = await _context.Ingredient
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.Ingredient.Remove(ingredient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngredientExists(Guid id)
        {
            return _context.Ingredient.Any(e => e.Id == id);
        }
    }
}
