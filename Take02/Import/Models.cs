using System;
using System.Collections.Generic;

namespace Take02.Import
{
    public class ImportRow
    {
        public string Library { get; set; }
        public string RecipeName { get; set; }
        public string Amount { get; set; }
        public string Unit { get; set; }
        public string Ingredient { get; set; }
        public int Index { get; set; }
        public int Garnish { get; set; }
        public string Instructions { get; set; }
    }

    public class StructuredImport
    {
        public ICollection<ImportLibrary> Libraries { get; set; }
    }
    
    public class ImportLibrary
    {
        public string LibraryName { get; set; }
        public ICollection<ImportRecipe> Recipes { get; set; }
    }

    public class ImportRecipe
    {
        public string RecipeName { get; set; }
        public string Instructions { get; set; }
        public ICollection<ImportIngredient> Ingredients { get; set; }
    }

    public class ImportIngredient
    {
        public string Amount { get; set; }
        public string Unit { get; set; }
        public string IngredientName { get; set; }
        public int Index { get; set; }
        public int Garnish { get; set; }
    }
}