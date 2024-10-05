using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PgLocator_web.Data;
using PgLocator_web.Models;
using System.Collections.Generic;
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
        public async Task<IActionResult> ApproveRejectPgOwner(int userId, string action)
        {
            // Find the user (PG Owner) by userId
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound("PG Owner not found");
            }

            // Check if the user role is PG Owner
            if (user.Role.ToLower() != "pgowner")
            {
                return BadRequest("User is not a PG Owner");
            }

            // Admin action: Approve or Reject the PG Owner
            if (action.ToLower() == "approve")
            {
                user.Status = "Approved";
            }
            else if (action.ToLower() == "reject")
            {
                user.Status = "Rejected";
            }
            else
            {
                return BadRequest("Invalid action. Use 'approve' or 'reject'");
            }

            // Update the user's status
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"PG Owner has been {action}d successfully.");
        }



        // View Approved PG Owners
        [HttpGet("ApprovedPgOwners")]
        public IActionResult GetApprovedPgOwners()
        {
            var pgOwners = _context.User.Where(u => u.Role == "PgOwner" && u.Status == "Approved").ToList();
            return Ok(pgOwners);
        }

        [HttpPost]
        [Route("ApproveRejectPg")]
        public async Task<IActionResult> ApproveRejectPg(int pgId, string action)
        {
            // Find the PG by pgId
            var pg = await _context.Pg.FindAsync(pgId);
            if (pg == null)
            {
                return NotFound("PG not found");
            }

            // Admin action: Approve or Reject the PG
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

            // Update the PG's status
            _context.Entry(pg).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"PG has been {action}d successfully.");
        }


        // View Approved PGs
        [HttpGet("ApprovedPgs")]
        public IActionResult GetApprovedPgs()
        {
            var pgs = _context.Pg.Where(p => p.Status == "Approved").ToList();
            return Ok(pgs);
        }


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
            pg.Address = updatedPg.Address;
            pg.City = updatedPg.City;
            pg.District = updatedPg.District;
            pg.Latitude = updatedPg.Latitude;
            pg.Longitude = updatedPg.Longitude;
            pg.Gender_perference = updatedPg.Gender_perference;
            pg.Amentities = updatedPg.Amentities;
            pg.Foodavailable = updatedPg.Foodavailable;
            pg.Meal = updatedPg.Meal;
            pg.Rules = updatedPg.Rules;

            _context.Entry(pg).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("PG Listing updated successfully");
        }



        // Delete Review
        [HttpDelete("DeleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = _context.Review.FirstOrDefault(r => r.Rid == id);
            if (review == null) return NotFound();

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();
            return Ok("Review deleted successfully");
        }

        // View Reviews for a specific PG
        [HttpGet("GetReviews/{pgId}")]
        public IActionResult GetReviews(int pgId)
        {
            var reviews = _context.Review.Where(r => r.Pid == pgId).ToList();
            return Ok(reviews);
        }

        // View PG Owner's Reviews
        [HttpGet("PgOwnerReviews/{pgOwnerId}")]
        public IActionResult GetPgOwnerReviews(int pgOwnerId)
        {
            var reviews = _context.Review.Where(r => r.Uid == pgOwnerId).ToList();
            return Ok(reviews);
        }
        //user search
        [HttpGet("usersearch")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers(string username)
        {
            // Fetch users whose first name or email contains the search string
            var user = await _context.User
                .Where(b => b.FirstName.Contains(username) || b.Email.Contains(username))
                .ToListAsync();


            // Filter users based on role, checking if the role is "User"
            var filteredusers = user.Where(user => user.Role == "user").ToList();

            if (filteredusers.Any())
            {
                return Ok(filteredusers);
            }

            return NotFound("No users with the specified role found.");
        }
        [HttpGet("pgownersearch")]
        public async Task<ActionResult<IEnumerable<User>>> SearchPgOwners(string username)
        {
            // Fetch users whose first name or email contains the search string
            var users = await _context.User
                .Where(b => b.FirstName.Contains(username) || b.Email.Contains(username))
                .ToListAsync();

            // Filter users based on role, checking if the role is "pgowner"
            var filteredPgOwners = users.Where(user => user.Role == "pgowner" && user.Status == "Approved").ToList();

            if (filteredPgOwners.Any())
            {
                return Ok(filteredPgOwners);
            }

            return NotFound("No PG owners with the specified role found.");
        }
        [HttpGet("searchaction")]
        public IActionResult SearchOwners(string? email = null)
        {
            // Check if _context is not null
            if (_context == null)
            {
                return StatusCode(500, "Database context is not initialized.");
            }

            // Start with all users whose status is pending and role is pgowner
            var users = _context.User
                .Where(u => u.Status.ToLower() == "pending" && u.Role.ToLower() == "pgowner")
                .AsQueryable();

            // Filter by email if provided
            if (!string.IsNullOrEmpty(email))
            {
                users = users.Where(u => u.Email.ToLower().Contains(email.ToLower()));
            }

            // Convert filtered results to a list
            var filteredUsers = users.ToList();

            // If no users match, return a 404 NotFound response
            if (!filteredUsers.Any())
            {
                return NotFound("No pending users found matching the search criteria.");
            }

            // Return the filtered list of users
            return Ok(filteredUsers);
        }




    }

}

