using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take02.Models;

namespace Take02.ViewModels
{
    public class RecipeViewModel
    {
        public Guid Id { get; set; }

        public Library Library { get; set; }

        public string Name { get; set; }

        public string Instructions { get; set; }

        public string Source { get; set; }

        public List<SelectListItem> LibrarySelectListItems { get; set; }

        public List<IngredientViewModel> IngredientViewModels { get; set; }

        public List<SelectListItem> ComponentSelectListItems { get; set; }

        public List<SelectListItem> UnitSelectListItems { get; set; }

        public bool ShowIngredients { get; set; }

        public RecipeViewModel()
        {
        }

        public RecipeViewModel(Recipe recipe, Library library)
        {
            Id = recipe.Id;
            Library = library;
            Name = recipe.Name;
            Instructions = recipe.Instructions;
            Source = recipe.Source;
        }
    }
}
