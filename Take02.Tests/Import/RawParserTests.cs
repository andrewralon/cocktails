using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Take02.Import;
using Take02.Models;

namespace Take02.Tests.Import
{
    public class RawParserTests
    {
        private readonly RawParser _sut = new RawParser();
        private const char Delimiter = '\t';

        private static readonly string[] FullRowFields = new [] 
        {
            "library",
            "recipe",
            "amount",
            "unit",
            "ingredient",
            "1",
            "0",
            "0",
            "instructions"
        };

        private static readonly string[] PartialRowFields = new [] 
        {
            "library",
            "recipe",
            "amount",
            "unit",
            "ingredient",
            "1",
            "0",
            "0"
        };

        [Fact]
        public void RawImport_GivenFullRow_ParsesAllValues()
        {
            var inputRow = String.Join(Delimiter, FullRowFields);
            var parsedList = _sut.ParseRawImport(new [] { inputRow}).ToList();

            Assert.Single(parsedList);
            var parsed = parsedList.First();
            Assert.NotNull(parsed);
            Assert.Equal(FullRowFields[0], parsed.Library);
            Assert.Equal(FullRowFields[1], parsed.RecipeName);
            Assert.Equal(FullRowFields[2], parsed.Amount);
            Assert.Equal(FullRowFields[3], parsed.Unit);
            Assert.Equal(FullRowFields[4], parsed.Ingredient);
            Assert.Equal(int.Parse(FullRowFields[5]), parsed.MixMethod);
            Assert.Equal(int.Parse(FullRowFields[6]), parsed.Index);
            Assert.Equal(int.Parse(FullRowFields[7]), parsed.Garnish);
            Assert.Equal(FullRowFields[8], parsed.Instructions);
        }

        [Fact]
        public void RawImport_GivenPartialRow_ParsesAvailableValues()
        {
            var inputRow = String.Join(Delimiter, PartialRowFields);
            var parsedList = _sut.ParseRawImport(new [] { inputRow}).ToList();

            Assert.Single(parsedList);
            var parsed = parsedList.First();
            Assert.NotNull(parsed);
            Assert.Equal(PartialRowFields[0], parsed.Library);
            Assert.Equal(PartialRowFields[1], parsed.RecipeName);
            Assert.Equal(PartialRowFields[2], parsed.Amount);
            Assert.Equal(PartialRowFields[3], parsed.Unit);
            Assert.Equal(PartialRowFields[4], parsed.Ingredient);
            Assert.Equal(int.Parse(PartialRowFields[5]), parsed.MixMethod);
            Assert.Equal(int.Parse(PartialRowFields[6]), parsed.Index);
            Assert.Equal(int.Parse(PartialRowFields[7]), parsed.Garnish);
            Assert.Null(parsed.Instructions);
        }

        [Fact]
        public void RawImport_GivenEmptyRow_IgnoresIt()
        {
            var parsedList = _sut.ParseRawImport(new [] { string.Empty }).ToList();
            Assert.Empty(parsedList);
        }

        [Fact]
        public void RawImport_GivenMultipleRows_CorrectlyParses()
        {
            var input = new [] 
            {
                string.Join(Delimiter, FullRowFields),
                string.Join(Delimiter, PartialRowFields),
                string.Empty
            };
            var parsedList = _sut.ParseRawImport(input).ToList();
            Assert.Equal(2, parsedList.Count);
        }

        [Fact]
        public void StructuredImport_GivenInput_StructuresCorrectly()
        {
            var libraries = new []
            {
                "Flagon Enterprises",
                "Landie.net: It's dot com"
            };

            var recipeNames = new []
            {
                "The GOB",
                "The Funky Fünke",
                "Gin-Michael",
                "Lou's Eel"
            };

            var ingredientTypes = new []
            {
                "Purp",
                "Cristal",
                "Courvoisier"
            };

            var importRows = new List<ImportRow>();
            foreach(var library in libraries)
            {
                foreach(var recipe in recipeNames)
                {
                    var index = 0;
                    foreach(var ingredientType in ingredientTypes)
                    {
                        importRows.Add(new ImportRow
                        {
                            Library = library,
                            RecipeName = recipe,
                            Amount = "A lot",
                            Unit = "of stuff",
                            Ingredient = ingredientType,
                            MixMethod = 1,
                            Index = index,
                            Garnish = 0,
                            Instructions = index++ == 0 ? "Instructions" : null
                        });
                    }
                }
            }

            var result = _sut.StructuredImport(importRows);

            Assert.Equal(2, result.Libraries.Count);

            var firstLibrary = result.Libraries.First();
            Assert.Equal(4, firstLibrary.Recipes.Count);

            var firstRecipe = firstLibrary.Recipes.First();
            Assert.Equal(3, firstRecipe.Ingredients.Count);
            Assert.Equal("Instructions", firstRecipe.Instructions);
        }
    }
}
