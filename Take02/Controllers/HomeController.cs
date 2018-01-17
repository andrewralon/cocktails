using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Take02.Models;
using Take02.ViewModels;

namespace Take02.Controllers
{
    public class HomeController : Controller
    {
        private readonly CocktailsContext _context;

        public HomeController(CocktailsContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mostRecipes = await _context
            .Recipe
            .GroupBy(a => a.LibraryId)
            .Select(a => new 
            {
                LibraryId = a.Key,
                Count = a.Count()
            })
            .OrderByDescending(a => a.Count)
            .Take(5)
            .ToListAsync();

            var libraryIds = mostRecipes.Select(a => a.LibraryId);

            var libraries = await _context.Library
            .Where(a => libraryIds.Contains(a.Id))
            .ToListAsync();

            var topLibraries = from summary in mostRecipes
                               join library in libraries on summary.LibraryId equals library.Id
                               select new LibrarySummaryViewModel
                               {
                                   LibraryId = library.Id,
                                   LibraryName = library.Name,
                                   RecipeCount = summary.Count
                               };

            var model = new HomeViewModel
            {
                TopLibraries = topLibraries.ToList()
            };

            return View(model);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
