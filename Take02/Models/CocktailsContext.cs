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
        public DbSet<Unit> Unit { get; set; }

        //public DbSet<ComponentViewModel> ComponentViewModel { get; set; }

        public CocktailsContext(DbContextOptions<CocktailsContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Library>()
                .Property(b => b.Name)
                .IsRequired();

            modelBuilder.Entity<Recipe>()
                .HasOne(a => a.Library)
                .WithMany(b => b.Recipes);

            modelBuilder.Entity<Component>()
                .HasOne(a => a.ComponentType);

            modelBuilder.Entity<Ingredient>()
                .HasOne(a => a.Recipe)
                .WithMany(b => b.Ingredients);

            modelBuilder.Entity<Ingredient>()
                .HasOne(a => a.Component);

            modelBuilder.Entity<Ingredient>()
                .HasOne(a => a.Unit);

            //modelBuilder.Ignore<ComponentViewModel>();
        }
    }
}
