using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Take02.Models
{
    public class Ingredient
    {
        public Guid Id { get; set; }
        [Required]
        public Guid RecipeId { get; set; }
        [Required]
        public Guid ComponentId { get; set; }
        public string Quantity { get; set; }
        [Required]
        public int UnitId { get; set; }
        public int Number { get; set; }

        public Component Component { get; set; }
        public Recipe Recipe { get; set; }
        public Unit Unit { get; set; }
    }
}
