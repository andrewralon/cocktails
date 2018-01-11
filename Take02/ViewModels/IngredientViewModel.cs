using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Take02.Models;

namespace Take02.ViewModels
{
    public class IngredientViewModel
    {
        public Guid Id { get; set; }

        public Guid RecipeId { get; set; }

        public string RecipeName { get; set; }

        public Guid ComponentId { get; set; }

        public string ComponentName { get; set; }

        public string Quantity { get; set; }

        public int UnitId { get; set; }

        public string UnitName { get; set; }

        public int Number { get; set; }

        public bool IsFirstIngredient { get; set; }

        public List<SelectListItem> ComponentSelectListItems { get; set; }

        public List<SelectListItem> RecipeSelectListItems { get; set; }

        public List<SelectListItem> UnitSelectListItems { get; set; }

        public string IngredientText { get { return string.Format("{0} {1} {2}", Quantity, UnitName, ComponentName); } }

        public IngredientViewModel()
        {
            Id = Guid.NewGuid();
            ComponentSelectListItems = new List<SelectListItem>();
            RecipeSelectListItems = new List<SelectListItem>();
            UnitSelectListItems = new List<SelectListItem>();
            IsFirstIngredient = false;
        }

        public IngredientViewModel(
            Ingredient ingredient, 
            Recipe recipe, 
            Component component,
            Unit unit)
            : base()
        {
            Id = ingredient.Id;
            RecipeId = recipe.Id;
            RecipeName = recipe.Name;
            ComponentId = component.Id;
            ComponentName = component.Name;
            Quantity = ingredient.Quantity;
            UnitId = unit.Id;
            UnitName = unit.Name;
            Number = ingredient.Number;
        }
    }
}
