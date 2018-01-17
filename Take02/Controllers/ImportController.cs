using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Take02.Import;
using Take02.Models;
using Take02.ViewModels;

namespace Take02.Controllers
{
    public class ImportController : Controller
    {
        private readonly IImporter _importer;

        public ImportController(IImporter importer)
        {
            _importer = importer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ImportViewModel
            {
                AreYouSerious = false
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ImportViewModel model)
        {
            if(!model.AreYouSerious)
            {
                throw new Exception("You weren't serious. Check the box if you're super sure you want to do this.");
            }
            await _importer.Import(model.ImportData);
            TempData["message"] = "Successfully bulk-imported data";
            return RedirectToAction("Index", "Home");
        }
    }
}
