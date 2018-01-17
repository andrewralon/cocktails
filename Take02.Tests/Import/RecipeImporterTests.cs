using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Take02.Import;
using Take02.Models;

namespace Take02.Tests.Import
{
    public class RecipeImporterTests
    {
        private const string LibraryName = "Testing Library";
        private static readonly Guid LibraryId = Guid.NewGuid();

        private readonly CocktailsContext _db;
        private readonly RecipeImporter _sut;

        public RecipeImporterTests()
        {
            var builder = new DbContextOptionsBuilder<CocktailsContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            _db = new CocktailsContext(builder.Options);
            _sut = new RecipeImporter(_db);

            _db.Library.Add(new Library
            {
                Id = LibraryId,
                Name = LibraryName
            });
            _db.SaveChanges();
        }

        [Fact]
        public async Task ImportRecipe_GivenExistingName_DoesNotReAdd()
        {
            var recipeName = "The GOB";
            await _db.Recipe.AddAsync(new Recipe
            {
                Id = Guid.NewGuid(),
                LibraryId = LibraryId,
                Name = recipeName,
                MixTypeId = 1,
                Instructions = "Instructions",
                Source = "Test"
            });
            await _db.SaveChangesAsync();

            await _sut.ImportRecipe(LibraryId, new ImportRecipe
            {
                RecipeName = recipeName,
                MixMethod = 1,
                Instructions = "Instructions"
            });
            Assert.Equal(1, _db.Recipe.Count());
        }

        [Fact]
        public async Task ImportRecipe_GivenNewName_Adds()
        {
            var recipeName = "The GOB";
            await _sut.ImportRecipe(LibraryId, new ImportRecipe
            {
                RecipeName = recipeName,
                MixMethod = 1,
                Instructions = "Instructions"
            });
            Assert.Equal(1, _db.Recipe.Count());
        }

        private static readonly IDictionary<string, int> UnitMap = new Dictionary<string, int>
        {
            { "oz", 1 }
        };

        private static readonly IDictionary<string, Guid> ComponentMap = new Dictionary<string, Guid>
        {
            { "LandieLiquor", Guid.NewGuid() }
        };

        private static IEnumerable<ImportIngredient> GenerateImportIngredients(int count) =>
            Enumerable.Range(0, count)
            .Select(a => new ImportIngredient
            {
                Amount = "1",
                Unit = UnitMap.First().Key,
                IngredientName = ComponentMap.First().Key,
                IngredientType = ComponentMap.First().Key,
                Index = 1,
                Garnish = 0
            });

        [Fact]
        public async Task ImportIngredients_GivenExistingRecipeId_DoesNothing()
        {
            var recipeId = Guid.NewGuid();
            await _db.Ingredient.AddAsync(new Ingredient
            {
                RecipeId = recipeId
            });
            await _db.SaveChangesAsync();

            var input = GenerateImportIngredients(2);
            await _sut.ImportIngredients(recipeId, input, UnitMap, ComponentMap);

            Assert.Equal(1, _db.Ingredient.Count());
        }

        [Fact]
        public async Task ImportIngredients_GivenAbsentRecipeId_AddsIngredients()
        {
            var recipeId = Guid.NewGuid();

            var input = GenerateImportIngredients(6);
            await _sut.ImportIngredients(recipeId, input, UnitMap, ComponentMap);

            Assert.Equal(6, _db.Ingredient.Count());
        }
    }
}