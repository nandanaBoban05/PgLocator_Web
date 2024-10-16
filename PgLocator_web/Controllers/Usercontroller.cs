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
                return Ok(_context.User.ToList());
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

                return Ok(new {message= "User Registered Successfully" });
            }
            else
            {
                return BadRequest("User already exists");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (login.Email == "admin@gmail.com" && login.Password == "Adminpass")
            {
                return Ok(new
                {
                    message = "Login successful",
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

            // Check if the PgOwner is approved or active
            if ((user.Role == "PgOwner" || user.Role == "pgowner") && (user.Status == "pending" || user.Status == "Pending" || user.Status == "rejected" || user.Status == "Rejected"))
            {
                return BadRequest(new { message = "PgOwner is not approved yet. Please wait for admin approval." });
            }

            // Check if the user account is active
            if ((user.Role == "User" || user.Role == "user") && (user.Status == "inactive" || user.Status == "Inactive" || user.Status == "banned" || user.Status == "Banned"))
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

        // Delete User
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Find the user by Uid
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Remove the user from the context
            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "User deleted successfully" });
        }




    }
}