using System;
using System.Collections;
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
        public async Task GivenExistingName_DoesNotReAdd()
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
        public async Task GivenNewName_Adds()
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
    }
}