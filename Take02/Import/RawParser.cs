using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Take02.Import;
using Take02.Models;

namespace Take02.Import
{
    public interface IRawParser
    {
        /// <summary>
        /// Takes input in the form of tab-delimited rows and turns it into usable classes
        /// </summary>
        IEnumerable<ImportRow> ParseRawImport(IEnumerable<string> rawImport);
        /// <summary>
        /// Takes parsed input and structures it into a more usable form
        /// </summary>
        StructuredImport StructuredImport(IEnumerable<ImportRow> parsedImport);
    }

    public class RawParser : IRawParser
    {
        private const char Delimiter = '\t';

        public IEnumerable<ImportRow> ParseRawImport(IEnumerable<string> rawImport)
        {
            foreach(var row in rawImport)
            {
                if(String.IsNullOrWhiteSpace(row))
                {
                    yield break;
                }

                var fields = row.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);
                // Borrowing scoped methods from functional programming
                Func<int, string> field = index => fields.Length < index + 1 ? null : fields[index].Trim();
                yield return new ImportRow
                {
                    Library = field(0),
                    RecipeName = field(1),
                    Amount = field(2),
                    Unit = field(3),
                    Ingredient = field(4),
                    Index = int.Parse(field(5)),
                    Garnish = int.Parse(field(6)),
                    Instructions = field(7)
                };
            }
        }

        public StructuredImport StructuredImport(IEnumerable<ImportRow> parsedImport)
        {
            var libraries = new List<ImportLibrary>();

            foreach(var libraryGrouping in parsedImport.GroupBy(a => a.Library))
            {
                var recipes = new List<ImportRecipe>();

                foreach(var recipeGrouping in libraryGrouping.GroupBy(a => a.RecipeName))
                {
                    var ingredients = recipeGrouping
                    .Select(a => new ImportIngredient
                    {
                        Amount = a.Amount,
                        Unit = a.Unit,
                        IngredientName = a.Ingredient,
                        Index = a.Index,
                        Garnish = a.Garnish
                    })
                    .ToList();

                    var instructions = recipeGrouping
                    .Select(a => a.Instructions)
                    .FirstOrDefault(a => !string.IsNullOrWhiteSpace(a));

                    recipes.Add(new ImportRecipe
                    {
                        RecipeName = recipeGrouping.Key,
                        Instructions = instructions,
                        Ingredients = ingredients
                    });
                }

                libraries.Add(new ImportLibrary
                {
                    LibraryName = libraryGrouping.Key,
                    Recipes = recipes
                });
            }

            return new StructuredImport
            {
                Libraries = libraries
            };
        }
    }
}
