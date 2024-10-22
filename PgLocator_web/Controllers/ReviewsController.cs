using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PgLocator_web.Data;
using PgLocator_web.Models;

namespace PgLocator_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReview()
        {
            return await _context.Review.ToListAsync();
        }

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Review.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Rid)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Rid }, review);
        }

       // DELETE: api/Reviews/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Review.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Review.Any(e => e.Rid == id);
        }
        [HttpPost("rev")]
        public async Task<ActionResult<Review>> UserReview(int uid, int pgid, [FromBody] Review review)
        {
            // Set Uid and Pgid from the parameters
            review.Uid = uid;
            review.Pgid = pgid;

            // Check if a review already exists for this user and PG
            var existingReview = await _context.Review
                .FirstOrDefaultAsync(r => r.Pgid == review.Pgid && r.Uid == review.Uid);

            if (existingReview != null)
            {
                return Conflict(new { message = "User has already reviewed this PG. Please update the existing review." });
            }

            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReview", new { id = review.Rid }, review);
        }
        // GET: api/Reviews/pgid/{pgid}
        [HttpGet("pgid/{pgid}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsByPgid(int pgid)
        {
            var reviews = await _context.Review
                .Where(r => r.Pgid == pgid)
                .ToListAsync();

            if (reviews == null || reviews.Count == 0)
            {
                return NotFound(new { message = "No reviews found for this PG." });
            }

            return Ok(reviews);
        }



        //respond to review:
        [HttpPut("respond/{reviewId}")]
        public async Task<IActionResult> RespondToReview(int reviewId, [FromBody] string response)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound();
            }

            review.Response = response;
            _context.Review.Update(review);
            await _context.SaveChangesAsync();

            return Ok(review);
        }



        //notifying admin a report of a review

        [HttpPut("report/{reviewId}")]
        public async Task<IActionResult> ReportReviewToAdmin(int reviewId)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound();
            }

            review.IsReported = true;
            review.ReportedToAdmin = true;  // Set this field to true to notify admin
            _context.Review.Update(review);
            await _context.SaveChangesAsync();

            // Optionally, you can trigger an email notification or a system alert here
            NotifyAdmin(review);

            return Ok(review);
        }

        // Dummy method for notifying admin (this can be an email, log, or entry in the database)
        private void NotifyAdmin(Review review)
        {
            try
            {
                var fromAddress = new MailAddress("pgowner@example.com", "PG Owner");
                var toAddress = new MailAddress("admin@example.com", "Admin");
                const string subject = "Review Report Notification";
                string body = $"A review (ID: {review.Rid}) for PG ID: {review.Pgid} has been reported by the owner.";

                var smtpClient = new SmtpClient
                {
                    Host = "smtp.mailtrap.io", // or your SMTP host
                    Port = 587,
                    EnableSsl = true,
                    Credentials = new NetworkCredential("username", "password") // Provide SMTP credentials
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtpClient.Send(message);
                }
            }
            catch (SmtpException smtpEx)
            {
                // Log or handle specific SMTP errors here
                Console.WriteLine($"SMTP Exception: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                // Log or handle general errors here
                Console.WriteLine($"General Exception: {ex.Message}");
            }
        }



        //get reported
        // GET: api/Reviews/reported-reviews
        [HttpGet("reported-reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReportedReviews()
        {
            var reportedReviews = await _context.Review
                .Where(r => r.ReportedToAdmin == true)
                .ToListAsync();

            return Ok(reportedReviews);
        }



    }

}
