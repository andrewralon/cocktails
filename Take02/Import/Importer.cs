using System;
using System.Linq;
using System.Threading.Tasks;
using Take02.Models;

namespace Take02.Import
{
    /// <summary>
    /// Top-level handler for batch imports. Batches as much DB IO as possible
    /// into bulk calls for known-small datasets, and iterates over longer sets
    /// </summary>
    public interface IImporter
    {
        Task Import(string data);
    }

    public class Importer : IImporter
    {
        private readonly IRawParser _rawParser;
        private readonly ILibraryImporter _libraryImporter;
        private readonly IComponentImporter _componentImporter;
        private readonly IRecipeImporter _recipeImporter;
        private readonly IUnitImporter _unitImporter;

        public Importer(IRawParser rawParser,
                        ILibraryImporter libraryImporter, 
                        IComponentImporter componentImporter,
                        IRecipeImporter recipeImporter,
                        IUnitImporter unitImporter)
        {
            _rawParser = rawParser;
            _libraryImporter = libraryImporter;
            _componentImporter = componentImporter;
            _recipeImporter = recipeImporter;
            _unitImporter = unitImporter;
        }

        public async Task Import(string data)
        {
            // Phase 0: Turn the big string into structured data

            var lines = data.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var parsed = _rawParser.ParseRawImport(lines);
            var structured = _rawParser.StructuredImport(parsed);
            
            // Phase 1: Setup peripheral schema components
            // (The steps of this Phase are parallelizable, but it's way easier to read and
            //  debug by blocking, and this is not a performance-critical operation yet.)
            
            var libraryNames = structured.Libraries.Select(a => a.LibraryName);
            var libraryMapping = await _libraryImporter.ImportLibraries(libraryNames);

            var allIngredients = structured.Libraries.SelectMany(library =>
                library.Recipes.SelectMany(recipe => recipe.Ingredients))
                // ToList() because the IEnumerable is reused
                .ToList();

            var allUnits = allIngredients.Select(a => a.Unit);
            var unitMap = await _unitImporter.ImportUnits(allUnits);

            var allComponentTypes = allIngredients.Select(a => a.IngredientType);
            await _componentImporter.ImportComponentTypes(allComponentTypes);
            var componentMap = await _componentImporter.ImportComponents(allIngredients);

            // Phase 2: Handle Recipes
            // (The iterations of this Phase are parallelizable, but it's way easier to read and
            //  debug by blocking, and this is not a performance-critical operation yet.)

            foreach(var library in structured.Libraries)
            {
                var libraryId = libraryMapping[library.LibraryName];
                foreach(var recipe in library.Recipes)
                {
                    var recipeId = await _recipeImporter.ImportRecipe(libraryId, recipe);
                    await _recipeImporter.ImportIngredients(recipeId, recipe.Ingredients,
                                                            unitMap, componentMap);
                }
            }
        }
    }
}