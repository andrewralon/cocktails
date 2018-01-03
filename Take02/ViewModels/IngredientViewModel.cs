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

        public Recipe Recipe { get; set; }

        public Component Component { get; set; }

        public string Quantity { get; set; }

        public Unit Unit { get; set; }

        public int Number { get; set; }

        public List<SelectListItem> ComponentSelectListItems { get; set; }

        public List<SelectListItem> RecipeSelectListItems { get; set; }

        public List<SelectListItem> UnitSelectListItems { get; set; }

        public string IngredientText { get; set; }

        public IngredientViewModel()
        {
            ComponentSelectListItems = new List<SelectListItem>();
            RecipeSelectListItems = new List<SelectListItem>();
            UnitSelectListItems = new List<SelectListItem>();
        }

        public IngredientViewModel(
            Ingredient ingredient, 
            Recipe recipe, 
            Component component,
            Unit unit)
            : base()
        {
            Id = ingredient.Id;
            Recipe = recipe;
            Component = component;
            Quantity = ingredient.Quantity;
            Unit = unit;
            Number = ingredient.Number;
            IngredientText = string.Format("{0} {1} {2}", Quantity, Unit.Name, Component.Name);
        }
    }
}
