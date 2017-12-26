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
    public class ComponentViewModel
    {
        public Guid Id { get; set; }

        public ComponentType ComponentType { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<SelectListItem> ComponentTypesSelectListItems { get; set; }

        public ComponentViewModel()
        {
        }

        public ComponentViewModel(Component component, ComponentType componentType)
        {
            Id = component.Id;
            ComponentType = componentType;
            Name = component.Name;
            Description = component.Description;
        }
    }
}
