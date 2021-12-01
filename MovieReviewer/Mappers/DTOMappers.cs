using MovieReviewer.Model;
using MovieReviewer.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewer.Mappers
{
    public class DTOMappers
    {
        public static MovieDTO MapMovieToMovieDTO(Movies movie)
        {
            if (movie == null)
                return null;

            return new MovieDTO
            {
                ID = movie.ID,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Score = movie.Score,
                Genre = MapGenreToGenreDTO(movie.Genre)
            };
        }

        public static GenreDTO? MapGenreToGenreDTO(Genre? genre)
        {
            if (genre == null)
                return null;

            return new GenreDTO
            {
                ID = genre.ID,
                Name = genre.Name
            };
        }
    }
}
