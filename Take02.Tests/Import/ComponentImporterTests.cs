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
    public class ComponentImporterTests
    {
        private readonly CocktailsContext _db;
        private readonly ComponentImporter _sut;

        public ComponentImporterTests()
        {
            var builder = new DbContextOptionsBuilder<CocktailsContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            _db = new CocktailsContext(builder.Options);
            _sut = new ComponentImporter(_db);
        }

        [Fact]
        public async Task ImportComponentTypes_GivenMixedSet_InsertsAndDoesNotDoubleInsert()
        {
            var existingNames = new []
            {
                "Absinthe",
                "Liqueur",
                "Rum"
            };

            var newNames = new []
            {
                "Fruit",
                "Gin",
                "Vodka"
            };

            await _db.ComponentType.AddRangeAsync(existingNames.Select(a => new ComponentType
            {
                Name = a
            }));
            await _db.SaveChangesAsync();

            await _sut.ImportComponentTypes(existingNames.Concat(newNames));
            Assert.Equal(6, _db.ComponentType.Count());
        }

        [Fact]
        public async Task ImportComponents_GivenMixedSet_InsertsAndDoesNotDoubleInsert()
        {
            var existingIngredientNames = new []
            {
                "Lucid Absinthe",
                "St Germaine",
                "Bacardi 151"
            };

            var newIngredientNames = new []
            {
                "Lemon Twist",
                "Beefeater",
                "Skyy"
            };

            var componentTypeName = "Generic test category";
            await _db.ComponentType.AddAsync(new ComponentType
            {
                Name = componentTypeName
            });
            await _db.SaveChangesAsync();

            var ingredients = existingIngredientNames
            .Concat(newIngredientNames)
            .Select(a => new ImportIngredient
            {
                IngredientName = a,
                IngredientType = componentTypeName
            });
            await _sut.ImportComponents(ingredients);
            Assert.Equal(6, _db.Component.Count());
        }
    }
}
