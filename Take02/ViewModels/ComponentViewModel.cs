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

        public int ComponentTypeId { get; set; }

        public string ComponentTypeName { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<SelectListItem> ComponentTypesSelectListItems { get; set; }

        public ComponentViewModel()
        {
            ComponentTypesSelectListItems = new List<SelectListItem>();
        }

        public ComponentViewModel(Component component, ComponentType componentType)
            : base()
        {
            Id = component.Id;
            ComponentTypeId = componentType.Id;
            ComponentTypeName = componentType.Name;
            Name = component.Name;
            Description = component.Description;
        }
    }
}
