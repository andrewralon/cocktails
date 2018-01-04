using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Take02.Models
{
    public class CocktailsContext : DbContext
    {
        public DbSet<Library> Library { get; set; }
        public DbSet<Recipe> Recipe { get; set; }
        public DbSet<Component> Component { get; set; }
        public DbSet<ComponentType> ComponentType { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        public DbSet<MixType> MixType { get; set; }
        public DbSet<Unit> Unit { get; set; }

        public CocktailsContext(DbContextOptions<CocktailsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Library>()
                .Property(b => b.Name)
                .IsRequired();
        }
    }
}
