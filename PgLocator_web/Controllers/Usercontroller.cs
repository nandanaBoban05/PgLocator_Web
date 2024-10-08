using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
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
    public class Usercontroller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Usercontroller(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get users
        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_context.User.ToList());
        }

        // Get users by Id
        [HttpGet]
        [Route("GetUser/{id}")]
        public IActionResult GetUsers(int id)
        {
            var user = _context.User.FirstOrDefault(x => x.Uid == id);
            if (user != null)
                return Ok();
            else
                return NoContent();
        }
        // User Registration
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the user already exists by email
            var objUser = _context.User.FirstOrDefault(x => x.Email == user.Email);
            if (objUser == null)
            {
                // Default status for users is "Active", but for PgOwners, it's "Pending" until admin approves
                string Status = user.Role == "PgOwner" ? "Pending" : "Active";

                _context.User.Add(new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    Email = user.Email,
                    Phone = user.Phone,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    Whatsapp = user.Whatsapp,
                    Chatlink = user.Chatlink,
                    Address = user.Address,
                    Status = Status,  
                    Password = user.Password,  
                });
                _context.SaveChanges();

                if (user.Role == "PgOwner")
                {
                    return Ok("PgOwner Registered Successfully. Awaiting admin approval.");
                }

                return Ok("User Registered Successfully");
            }
            else
            {
                return BadRequest("User already exists");
            }
        }

        // User Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if(login.Email == "admin@gmail.com" && login.Password == "Adminpass")
            {
                return Ok(new
                {
                    message = "Login sucessful",
                    userId = "Admin",
                    role = "admin"
                });
            }
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid username or password" });
            }
            // Admins should not go through the 'approval' process
            if (user.Role == "admin")
            {
                return Ok(new
                {
                    message = "Login successful",
                    userId = user.Uid,
                    role = user.Role
                });
            }
            // Check if the user is active (for regular users) or approved (for PgOwners)
            if (user.Role == "PgOwner" && user.Status != "Approved")
            {
                return BadRequest(new { message = "PgOwner is not approved yet. Please wait for admin approval." });
            }

            if (user.Role == "User" && user.Status != "Active")
            {
                return BadRequest(new { message = "User account is inactive." });
            }

            // Return success message and userId
            return Ok(new
            {
                message = "Login successful",
                userId = user.Uid,
                role = user.Role   
            });
        }

        // User Edit
        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User updatedUser)
        {
            // Find the user by Uid
            var user = await _context.User.FirstOrDefaultAsync(u => u.Uid == id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Update the user's properties
            user.FirstName = updatedUser.FirstName ?? user.FirstName;
            user.LastName = updatedUser.LastName ?? user.LastName;
            user.Role = updatedUser.Role ?? user.Role;
            user.Email = updatedUser.Email ?? user.Email;
            user.Phone = updatedUser.Phone ?? user.Phone;
            user.Gender = updatedUser.Gender ?? user.Gender;
            user.Dob = updatedUser.Dob ?? user.Dob;
            user.Whatsapp = updatedUser.Whatsapp ?? user.Whatsapp;
            user.Chatlink = updatedUser.Chatlink ?? user.Chatlink;
            user.Address = updatedUser.Address ?? user.Address;
            user.Status = updatedUser.Status ?? user.Status;

            // Optionally update the password (if needed, remember to hash the password)
            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                user.Password = updatedUser.Password;  // Hashing should be done here in real-world cases
            }

            // Save changes
            await _context.SaveChangesAsync();

            return Ok(new { message = "User updated successfully" });
        }

        // Add Review
        [HttpPost("Review")]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            // Check if user is active
            var user = await _context.User.FindAsync(review.Uid);
            if (user == null || user.Status != "Active")
            {
                return BadRequest(new { message = "Only active users can add reviews." });
            }

            review.Reviewdate = DateTime.UtcNow; // Set review date to now
            _context.Review.Add(review);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Review added successfully", review });
        }

        // Edit Review
        [HttpPut("Review/{id}")]
        public async Task<IActionResult> EditReview(int id, [FromBody] Review updatedReview)
        {
            var review = await _context.Review.FirstOrDefaultAsync(r => r.Rid == id);

            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            review.Rating = updatedReview.Rating ?? review.Rating;
            review.Reviewteaxt = updatedReview.Reviewteaxt ?? review.Reviewteaxt;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Review updated successfully", review });
        }

        // View All Reviews for a Specific Pg (by Pid)
        [HttpGet("Review/{pid}")]
        public async Task<IActionResult> ViewReviews(int pid)
        {
            var reviews = await _context.Review
                .Where(r => r.Pid == pid)
                .ToListAsync();

            if (reviews == null || reviews.Count == 0)
            {
                return NotFound(new { message = "No reviews found for this Pg" });
            }

            return Ok(reviews);
        }

        // Delete Review
        [HttpDelete("Review/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Review.FirstOrDefaultAsync(r => r.Rid == id);

            if (review == null)
            {
                return NotFound(new { message = "Review not found" });
            }

            _context.Review.Remove(review);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Review deleted successfully" });
        }
    }
}