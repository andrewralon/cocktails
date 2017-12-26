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

        public IngredientViewModel()
        {
        }

        public IngredientViewModel(
            Ingredient ingredient, 
            Recipe recipe, 
            Component component,
            Unit unit)
        {
            Id = ingredient.Id;
            Recipe = recipe;
            Component = component;
            Quantity = ingredient.Quantity;
            Unit = unit;
            Number = ingredient.Number;
        }
    }
}
