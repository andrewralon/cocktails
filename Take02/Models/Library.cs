using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Take02.Models
{
    public class Library
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<Recipe> Recipes { get; set; }
    }
}
