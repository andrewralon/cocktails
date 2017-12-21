using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Take02.Models
{
    [NotMapped]
    public class ComponentViewModel
    {
        // Blatantly copied from Component's model
        public Guid Id { get; set; }
        [Required]
        public int ComponentTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        //public Guid Id { get; set; }
        //[Display(Name = "Component Type")]
        //public ComponentType ComponentType { get; set; }
        [NotMapped, Display(Name = "Component Type")]
        public IEnumerable<SelectListItem> ComponentTypes { get; set; }
    }
}
