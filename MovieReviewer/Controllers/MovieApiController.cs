using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieReviewer.DAL;
using MovieReviewer.Mappers;
using MovieReviewer.Model;
using MovieReviewer.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReviewer.Controllers
{
    [Route("api/movie")]
    [ApiController]
    public class MovieApiController : Controller
    {
        private MovieReviewerContext _context;
        public MovieApiController(MovieReviewerContext context)
        {
            this._context = context;
        }
        public async Task<IActionResult> Get()
        {
            var movies = new List<MovieDTO>();
            await _context.Movies.Include(m => m.Genre).ForEachAsync(m => movies.Add(DTOMappers.MapMovieToMovieDTO(m)));

            return Ok(movies);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Movies movie)
        {
            _context.Movies.Add(movie);
            _context.SaveChanges();

            var createdMovie = _context.Movies.Include(m => m.Genre).First(m => m.ID == movie.ID);
            return Created("", DTOMappers.MapMovieToMovieDTO(createdMovie));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var movie = _context.Movies.Find(id);

            if (movie == null)
                return NotFound();

            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
