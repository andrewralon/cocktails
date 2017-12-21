using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Take02.Models
{
    public class Component
    {
        public Guid Id { get; set; }
        [Required]
        public int ComponentTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        //[NotMapped]
        public ComponentType ComponentType { get; set; }
        //[NotMapped]
        //public List<SelectListItem> ComponentTypes { get; set; }
    }
}
