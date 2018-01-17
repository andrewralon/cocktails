using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using Take02.Import;
using Take02.Models;

namespace Take02.Tests.Import
{
    public class ImporterTests
    {
        private readonly IRawParser _rawParser;
        private readonly ILibraryImporter _libraryImporter;
        private readonly IComponentImporter _componentImporter;
        private readonly IRecipeImporter _recipeImporter;
        private readonly IUnitImporter _unitImporter;

        private readonly Importer _sut;

        public ImporterTests()
        {
            _rawParser = Substitute.For<IRawParser>();
            _libraryImporter = Substitute.For<ILibraryImporter>();
            _componentImporter = Substitute.For<IComponentImporter>();
            _recipeImporter = Substitute.For<IRecipeImporter>();
            _unitImporter = Substitute.For<IUnitImporter>();

            _sut = new Importer(_rawParser, _libraryImporter, _componentImporter,
                                       _recipeImporter, _unitImporter);
        }

        private static readonly string[] InputLines = new []
        {
            "first line",
            "second line",
            "third line"
        };

        [Fact]
        public async Task GivenHappyPathData_ExecutesHappyPath()
        {
            var singleLine = String.Join(Environment.NewLine, InputLines);

            _rawParser
            .ParseRawImport(Arg.Any<IEnumerable<string>>())
            .Returns(Enumerable.Empty<ImportRow>());

            var libraryName = "Flagon Enterprises";
            var unitName = "pint";
            var ingredientName = "The GOB";

            var importModel = new StructuredImport
            {
                Libraries = new []
                {
                    new ImportLibrary
                    {
                        LibraryName = libraryName,
                        Recipes = new []
                        {
                            new ImportRecipe
                            {
                                RecipeName = "Test Recipe",
                                MixMethod = 1,
                                Instructions = "Instructions",
                                Ingredients = new []
                                {
                                    new ImportIngredient
                                    {
                                        Amount = "1",
                                        Unit = unitName,
                                        IngredientName = ingredientName,
                                        IngredientType = ingredientName,
                                        Index = 0,
                                        Garnish = 0
                                    },
                                    new ImportIngredient
                                    {
                                        Amount = "1",
                                        Unit = unitName,
                                        IngredientName = ingredientName,
                                        IngredientType = ingredientName,
                                        Index = 1,
                                        Garnish = 0
                                    }
                                }
                            }
                        }
                    }
                }
            };

            _rawParser
            .StructuredImport(Arg.Any<IEnumerable<ImportRow>>())
            .Returns(importModel);

            _libraryImporter
            .ImportLibraries(Arg.Any<IEnumerable<string>>())
            .Returns(new Dictionary<string, Guid>
            {
                { libraryName, Guid.NewGuid() }
            });

            _unitImporter
            .ImportUnits(Arg.Any<IEnumerable<string>>())
            .Returns(new Dictionary<string, int>
            {
                { unitName, 1 }
            });

            _componentImporter
            .ImportComponents(Arg.Any<IEnumerable<ImportIngredient>>())
            .Returns(new Dictionary<string, Guid>
            {
                { ingredientName, Guid.NewGuid() }
            });

            _recipeImporter
            .ImportRecipe(Arg.Any<Guid>(), Arg.Any<ImportRecipe>())
            .Returns(Guid.NewGuid());

            await _sut.Import(singleLine);

            _rawParser
            .Received(1)
            .ParseRawImport(Arg.Any<IEnumerable<string>>());

            _rawParser
            .Received(1)
            .StructuredImport(Arg.Any<IEnumerable<ImportRow>>());

            await _libraryImporter
            .Received(1)
            .ImportLibraries(Arg.Any<IEnumerable<string>>());

            await _unitImporter
            .Received(1)
            .ImportUnits(Arg.Any<IEnumerable<string>>());

            await _componentImporter
            .Received(1)
            .ImportComponents(Arg.Any<IEnumerable<ImportIngredient>>());
            
            await _recipeImporter
            .Received(1)
            .ImportRecipe(Arg.Any<Guid>(), Arg.Any<ImportRecipe>());

            await _recipeImporter
            .Received(1)
            .ImportIngredients(Arg.Any<Guid>(), Arg.Any<IEnumerable<ImportIngredient>>(),
                               Arg.Any<IDictionary<string, int>>(),
                               Arg.Any<IDictionary<string, Guid>>());
        }
    }
}
