using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Take02.Models;

namespace Take02.ViewModels
{
    [NotMapped]
    public class ComponentViewModel
    {
        public Guid Id { get; set; }

        [Required, Display(Name = "Type")]
        public int ComponentTypeId { get; set; }

        [Required, Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [NotMapped]
        public List<SelectListItem> ComponentTypesSelectListItems { get; set; }

        public ComponentViewModel()
        {
        }

        public ComponentViewModel(Component component)
        {
            Id = component.Id;
            ComponentTypeId = component.ComponentTypeId;
            Name = component.Name;
            Description = component.Description;
        }
    }
}
