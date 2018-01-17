using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Take02.Models;

namespace Take02.Import
{
    public interface IComponentImporter
    {
        /// <summary>
        /// An idempotent operation which takes in a set of ComponentType names
        /// and verifies that they all exist in the database
        /// </summary>
        Task ImportComponentTypes(IEnumerable<string> componentTypeNames);

        /// <summary>
        /// And idempotent operation which takes in a set of ImportIngredients
        /// and verifies that they all exist in the database. Assumes that
        /// their ComponentTypes are already imported.
        /// </summary>
        Task ImportComponents(IEnumerable<ImportIngredient> ingredients);
    }

    public class ComponentImporter : IComponentImporter
    {
        private readonly CocktailsContext _context;

        public ComponentImporter(CocktailsContext context)
        {
            _context = context;
        }

        public async Task ImportComponentTypes(IEnumerable<string> componentTypeNames)
        {
            var existingComponentTypes = await _context.ComponentType.ToListAsync();

            var newComponentTypes = componentTypeNames
            .Distinct()
            .Except(existingComponentTypes.Select(a => a.Name))
            .Select(a => new ComponentType
            {
                Name = a
            });

            await _context.ComponentType.AddRangeAsync(newComponentTypes);
            await _context.SaveChangesAsync();
        }

        public async Task ImportComponents(IEnumerable<ImportIngredient> ingredients)
        {
            var componentTypes = await _context.ComponentType.ToListAsync();
            var componentTypeMap = componentTypes.ToDictionary(a => a.Name, a => a.Id);

            var existingComponentNames = await _context
            .Component
            .Select(a => a.Name)
            .ToListAsync();

            var newComponents = ingredients
            .GroupBy(a => a.IngredientName)
            .Where(ingredientGrouping => !existingComponentNames.Contains(ingredientGrouping.Key))
            .Select(a => new Component
            {
                Id = Guid.NewGuid(),
                ComponentTypeId = componentTypeMap[a.First().IngredientType],
                Name = a.Key,
                Description = a.Key
            });

            await _context.Component.AddRangeAsync(newComponents);
            await _context.SaveChangesAsync();
        }
    }
}