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
    public class UnitImporterTests
    {
        private readonly CocktailsContext _db;
        private readonly UnitImporter _sut;

        public UnitImporterTests()
        {
            var builder = new DbContextOptionsBuilder<CocktailsContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            _db = new CocktailsContext(builder.Options);
            _sut = new UnitImporter(_db);
        }

        [Fact]
        public async Task GivenMixedSet_DoesNotDoubleInsert()
        {
            var existingUnits = new []
            {
                "oz",
                "tsp",
                "g"
            };

            var newUnits = new []
            {
                "pint",
                "gallon",
                "gross"
            };

            await _db.Unit.AddRangeAsync(existingUnits.Select(a => new Unit
            {
                Name = a
            }));
            await _db.SaveChangesAsync();

            await _sut.ImportUnits(existingUnits.Concat(newUnits));
            Assert.Equal(6, _db.Unit.Count());
        }
    }
}
