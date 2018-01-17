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
    public class LibraryImporterTests
    {
        private readonly CocktailsContext _db;
        private readonly LibraryImporter _sut;

        public LibraryImporterTests()
        {
            var builder = new DbContextOptionsBuilder<CocktailsContext>()
                                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            _db = new CocktailsContext(builder.Options);
            _sut = new LibraryImporter(_db);
        }

        [Fact]
        public async Task GivenExistingName_DoesNotReAdd()
        {
            var libraryName = "Flagon Enterprises";
            await _db.Library.AddAsync(new Library
            {
                Id = Guid.NewGuid(),
                Name = libraryName
            });
            await _db.SaveChangesAsync();

            await _sut.ImportLibraries(new [] { libraryName });
            Assert.Equal(1, _db.Library.Count());
        }

        [Fact]
        public async Task GivenNewName_Adds()
        {
            var libraryName = "Flagon Enterprises";

            await _sut.ImportLibraries(new [] { libraryName });
            Assert.Equal(1, _db.Library.Count());
        }

        [Fact]
        public async Task GivenMixedSet_CorrectlyAssemblesResult()
        {
            var existingLibraryName = "Flagon Enterprises";
            await _db.Library.AddAsync(new Library
            {
                Id = Guid.NewGuid(),
                Name = existingLibraryName
            });
            await _db.SaveChangesAsync();

            var newLibraryName = "Landie Enterprises";

            var result = await _sut.ImportLibraries(new [] { existingLibraryName, newLibraryName });

            Assert.Equal(2, _db.Library.Count());
            Assert.True(result.ContainsKey(existingLibraryName));
            Assert.True(result.ContainsKey(newLibraryName));
        }
    }
}