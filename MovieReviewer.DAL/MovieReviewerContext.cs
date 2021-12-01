using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieReviewer.Model;

namespace MovieReviewer.DAL
{
    public class MovieReviewerContext : DbContext
    {
        public MovieReviewerContext (DbContextOptions<MovieReviewerContext> options)
            : base(options)
        {
        }

        public DbSet<Movies> Movies { get; set; }
        public DbSet<Review> Rewiews { get; set; }
        public DbSet<Genre> Genres { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Genre>().HasData(new List<Genre>()
            {
                new Genre {ID = 1, Name = "Action"},
                new Genre {ID = 2, Name = "Drama"},
                new Genre {ID = 3, Name = "Comedy"},
                new Genre {ID = 4, Name = "Fantasy"},
                new Genre {ID = 5, Name = "Advanture"},
                new Genre {ID = 6, Name = "Horror"},
            });
        }
    }
}
