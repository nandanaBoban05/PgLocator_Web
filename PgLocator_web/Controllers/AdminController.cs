using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PgLocator_web.Data;
using PgLocator_web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PgLocator_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }


        // Approve or reject PG Owner
        [HttpPost]
        [Route("ApproveRejectPgOwner")]
        public async Task<IActionResult> ApproveRejectPgOwner(int ownerId, string action)
        {
            var owner = await _context.Owner.FindAsync(ownerId);
            if (owner == null)
            {
                return NotFound("PG Owner not found");
            }

            var login = await _context.Login.FindAsync(owner.Lid); 
            if (login == null)
            {
                return NotFound("Login entry not found for this PG owner");
            }

            // Admin action: Approve or Reject
            if (action.ToLower() == "approve")
            {
                login.Role = "Approved";
            }
            else if (action.ToLower() == "reject")
            {
                login.Role = "Rejected";
            }
            else
            {
                return BadRequest("Invalid action. Use 'approve' or 'reject'");
            }

            _context.Entry(login).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"PG Owner {action}d successfully");
        }

        // View all approved PG Owners
        [HttpGet]
        [Route("ViewApprovedPgOwners")]
        public async Task<IActionResult> ViewApprovedPgOwners()
        {
            var approvedOwners = await _context.Owner
                .Where(o => _context.Login.Any(l => l.Lid == o.Lid && l.Role == "Approved"))
                .ToListAsync();

            return Ok(approvedOwners);
        }

        // Approve or reject PG Listing
        [HttpPost]
        [Route("ApproveRejectPg")]
        public async Task<IActionResult> ApproveRejectPg(int pgId, string action)
        {
            var pg = await _context.Pg.FindAsync(pgId);
            if (pg == null)
            {
                return NotFound("PG Listing not found");
            }

            // Admin action: Approve or Reject
            if (action.ToLower() == "approve")
            {
                pg.Status = "Approved";
            }
            else if (action.ToLower() == "reject")
            {
                pg.Status = "Rejected";
            }
            else
            {
                return BadRequest("Invalid action. Use 'approve' or 'reject'");
            }

            _context.Entry(pg).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"PG Listing {action}d successfully");
        }

        // View all approved PG Listings
        [HttpGet]
        [Route("ViewApprovedPg")]
        public async Task<IActionResult> ViewApprovedPg()
        {
            var approvedPgs = await _context.Pg
                .Where(pg => pg.Status == "Approved")
                .ToListAsync();

            return Ok(approvedPgs);
        }


        // Edit PG Listing
        [HttpPut]
        [Route("EditPg")]
        public async Task<IActionResult> EditPg(int pgId, Pg updatedPg)
        {
            if (pgId != updatedPg.Pgid)
            {
                return BadRequest("PG ID mismatch");
            }

            var pg = await _context.Pg.FindAsync(pgId);
            if (pg == null)
            {
                return NotFound("PG Listing not found");
            }

            // Update properties
            pg.Pgname = updatedPg.Pgname;
            pg.Description = updatedPg.Description;
            pg.Adress = updatedPg.Adress;
            pg.Pin = updatedPg.Pin;
            pg.Gender = updatedPg.Gender;
            pg.Image = updatedPg.Image;
            pg.Amentities = updatedPg.Amentities;
            pg.Foodavailable = updatedPg.Foodavailable;
            pg.Meal = updatedPg.Meal;
            pg.Status = updatedPg.Status;
            pg.Rules = updatedPg.Rules;
            pg.District = updatedPg.District;
            pg.Place = updatedPg.Place;
            pg.Latitude = updatedPg.Latitude;
            pg.Longitude = updatedPg.Longitude;

            _context.Entry(pg).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("PG Listing updated successfully");
        }

        // Delete PG Listing
        [HttpDelete]
        [Route("DeletePg/{pgId}")]
        public async Task<IActionResult> DeletePg(int pgId)
        {
            var pg = await _context.Pg.FindAsync(pgId);
            if (pg == null)
            {
                return NotFound("PG Listing not found");
            }

            _context.Pg.Remove(pg);
            await _context.SaveChangesAsync();

            return Ok("PG Listing deleted successfully");
        }

        // View all reviews
        [HttpGet]
        [Route("ViewReviews")]
        public async Task<IActionResult> ViewReviews()
        {
            var reviews = await _context.Review.ToListAsync();
            return Ok(reviews);
        }

        // Delete a review
        [HttpDelete]
        [Route("DeleteReview/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            return Ok("Review deleted successfully");
        }

        // Edit User (for PG Owner or User)
        [HttpPut]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser(int userId, Login updatedLogin)
        {
            var user = await _context.Login.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Username = updatedLogin.Username; 
            user.Password = updatedLogin.Password;
            user.Role = updatedLogin.Role;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("User updated successfully");
        }


        // Delete User
        [HttpDelete]
        [Route("DeleteUser/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var user = await _context.Login.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            _context.Login.Remove(user);
            await _context.SaveChangesAsync();

            return Ok("User deleted successfully");
        }
    }
}