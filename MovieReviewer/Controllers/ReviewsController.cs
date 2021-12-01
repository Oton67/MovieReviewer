using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieReviewer.DAL;
using MovieReviewer.Model;

namespace MovieReviewer
{
    public class ReviewsController : Controller
    {
        private readonly MovieReviewerContext _context;

        public ReviewsController(MovieReviewerContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public async Task<IActionResult> Index(int? id)
        {
            var movieReviewerContext = _context.Rewiews.Include(r => r.Movie);

            if (id == null) 
            {
                return View(await movieReviewerContext.ToListAsync());
            }

            var reviewQuery = _context.Rewiews.OrderByDescending(r => r.ID).Include(m => m.Movie).Where(m=>m.MovieID == id);
            var model = reviewQuery.ToList();

            return View("Index", model);
        }

        // GET: Reviews/Details/id
        [Route("review-details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Rewiews
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["MovieID"] = new SelectList(_context.Movies, "ID", "Title");
            return View();
        }

        // POST: Reviews/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Text,Score,DateCreated,User,MovieID")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                review.DateCreated = DateTime.Now;
                if (User.Identity.Name != null) review.User = User.Identity.Name;
                else review.User = "Anonymous";
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MovieID"] = new SelectList(_context.Movies, "ID", "Title", review.MovieID);
            return View(review);
        }

        // GET: Reviews/Edit/id
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Rewiews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            if (User.Identity.Name == review.User || User.IsInRole("Admin"))
            {
                ViewData["MovieID"] = new SelectList(_context.Movies, "ID", "Title", review.MovieID);
                return View(review);
            }
            else return NotFound("You can't EDIT other people's reviews!");
        }

        // POST: Reviews/Edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Text,Score,DateCreated,User,MovieID")] Review review)
        {
            
            if (id != review.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                review.User = User.Identity.Name;
                review.DateCreated = DateTime.Now;
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.ID))
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
            ViewData["MovieID"] = new SelectList(_context.Movies, "ID", "Title", review.MovieID);
            return View(review);      
        }

        // GET: Reviews/Delete/id
        [Authorize(Roles = "User, Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Rewiews
                .Include(r => r.Movie)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (review == null)
            {
                return NotFound();
            }
            if (User.Identity.Name == review.User || User.IsInRole("Admin"))
            {
                return View(review);
            }
            else return NotFound("You can't DELETE other people's reviews!");
        }

        // POST: Reviews/Delete/id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Rewiews.FindAsync(id);
            _context.Rewiews.Remove(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Rewiews.Any(e => e.ID == id);
        }
    }
}
