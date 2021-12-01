using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieReviewer.Model;
using MovieReviewer.DAL;
using MovieReviewer.Models;
using Microsoft.AspNetCore.Authorization;

namespace MovieReviewer.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MovieReviewerContext _context;

        public MoviesController(MovieReviewerContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(MovieFilterModel filter)
        {
            var movieQuery = _context.Movies.Include(m => m.Genre).OrderByDescending(m => m.ID).AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Title))
                movieQuery = movieQuery.Where(m => m.Title.ToLower().Contains(filter.Title.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.Genre))
                movieQuery = movieQuery.Where(m => m.Genre != null && m.Genre.Name.ToLower().Contains(filter.Genre.ToLower()));


            if (!string.IsNullOrWhiteSpace(filter.ReleaseYear))
                movieQuery = movieQuery.Where(m => m.ReleaseDate.Year.ToString().Contains(filter.ReleaseYear));

            if (!string.IsNullOrWhiteSpace(filter.Score))
                movieQuery = movieQuery.Where(m => m.Score.ToString().Contains(filter.Score));


            var movies = movieQuery.ToList();
            return View("Index", movies);
        }

        // GET: Movies/Details/id
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movies = await _context.Movies.Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movies == null)
            {
                return NotFound();
            }

            return View(movies);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.GenreOptions = FillDropDownValuesWithGenres();
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,Description,GenreID,ReleaseDate,Score")] Movies movies)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movies);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movies);
        }

        // GET: Movies/Edit/id
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.GenreOptions = FillDropDownValuesWithGenres();
            if (id == null)
            {
                return NotFound();
            }

            var movies = await _context.Movies.FindAsync(id);
            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }

        // POST: Movies/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,Description,GenreID,ReleaseDate,Score")] Movies movies)
        {
            if (id != movies.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movies);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesExists(movies.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movies);
        }

        // GET: Movies/Delete/id
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movies = await _context.Movies
                .FirstOrDefaultAsync(m => m.ID == id);
            if (movies == null)
            {
                return NotFound();
            }

            return View(movies);
        }

        // POST: Movies/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movies = await _context.Movies.FindAsync(id);
            _context.Movies.Remove(movies);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MoviesExists(int id)
        {
            return _context.Movies.Any(e => e.ID == id);
        }

        private List<SelectListItem> FillDropDownValuesWithGenres()
        {
            var list = new List<SelectListItem>();

            var selectListItem = new SelectListItem();
            selectListItem.Text = "- Select - ";
            selectListItem.Value = "";

            list.Add(selectListItem);

            foreach (var genre in _context.Genres)
            {
                selectListItem = new SelectListItem(genre.Name, genre.ID.ToString());
                list.Add(selectListItem);
            }

            return list;
        }
    }
}
