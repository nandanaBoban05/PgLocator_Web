using System;
using System.Collections.Generic;
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
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _context.User.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (existingUser != null)
            {
                return BadRequest("User already exists with the same email address");
            }

            var login = new Login
            {
                Username = user.Email, 
                Password = user.Password,
                Role = "User" 
            };

            _context.Login.Add(login);
            await _context.SaveChangesAsync(); 

            user.Lid = login.Lid; 
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }


        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Login login)
        {
            var user = _context.User.FirstOrDefault(x => x.Name == login.Username && x.Password == login.Password);
            if (user != null)
            {
                return Ok(new { user.Uid, user.Name, user.Email });
            }
            return Unauthorized("Invalid credentials");
        }




        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Uid)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Uid }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Uid == id);
        }

        // Edit user profile
        [HttpPut]
        [Route("EditProfile/{userId}")]
        public async Task<IActionResult> EditProfile(int userId, User updatedUser)
        {
            var user = await _context.User.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Phone = updatedUser.Phone;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("User profile updated successfully");
        }

        // View PG owners
        [HttpGet]
        [Route("ViewPgOwners")]
        public async Task<IActionResult> ViewPgOwners()
        {
            var owners = await _context.Owner.ToListAsync();
            return Ok(owners);
        }

        // Add a review
        [HttpPost]
        [Route("AddReview")]
        public async Task<IActionResult> AddReview(Review review)
        {
            _context.Review.Add(review);
            await _context.SaveChangesAsync();
            return Ok("Review added successfully");
        }

        // Edit a review
        [HttpPut]
        [Route("EditReview/{reviewId}")]
        public async Task<IActionResult> EditReview(int reviewId, Review updatedReview)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review == null)
            {
                return NotFound("Review not found");
            }

            review.Rating = updatedReview.Rating;
            review.Reviewteaxt = updatedReview.Reviewteaxt;
            review.Reviewdate = updatedReview.Reviewdate;

            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Review updated successfully");
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
    }
}
    
