using System;
using System.Collections.Generic;

namespace Take02.ViewModels
{
    public class HomeViewModel
    {
        public ICollection<LibrarySummaryViewModel> TopLibraries { get; set; }
    }

    public class LibrarySummaryViewModel
    {
        public Guid LibraryId { get; set; }
        public string LibraryName { get; set; }
        public int RecipeCount { get; set; }
    }
}