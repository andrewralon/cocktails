using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Take02.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }

        [Required]
        public Guid LibraryId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Instructions { get; set; }

        public string Source { get; set; }
    }
}
