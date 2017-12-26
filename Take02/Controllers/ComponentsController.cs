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
    public class ComponentsController : Controller
    {
        private readonly CocktailsContext _context;

        public ComponentsController(CocktailsContext context)
        {
            _context = context;
        }

        // GET: Components
        public async Task<IActionResult> Index()
        {
            var models = await Helper.GetComponentViewModelsAsync(_context);
            return View(models);
        }

        // GET: Components/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await Helper.GetComponentViewModelAsync(_context, id.Value);
            return View(model);
        }

        // GET: Components/Create
        public IActionResult Create()
        {
            var model = new ComponentViewModel();
            model.ComponentTypesSelectListItems = Helper.GetComponentTypeSelectListItems(_context);
            return View(model);
        }

        // POST: Components/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ComponentTypeId,Name,Description")] Component component)
        {
            if (ModelState.IsValid)
            {
                component.Id = Guid.NewGuid();
                _context.Add(component);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(component);
        }

        // GET: Components/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await Helper.GetComponentViewModelAsync(_context, id.Value);
            model.ComponentTypesSelectListItems = Helper.GetComponentTypeSelectListItems(_context);
            return View(model);
        }

        // POST: Components/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,ComponentTypeId,Name,Description")] Component component)
        {
            if (id != component.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(component);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComponentExists(component.Id))
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
            return View(component);
        }

        // GET: Components/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await Helper.GetComponentViewModelAsync(_context, id.Value);
            return View(model);
        }

        // POST: Components/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var component = await _context.Component
                .SingleOrDefaultAsync(m => m.Id == id);
            _context.Component.Remove(component);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComponentExists(Guid id)
        {
            return _context.Component.Any(e => e.Id == id);
        }
    }
}
