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
    public class OwnersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OwnersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Register PG Owner
        [HttpPost("Register")]
        public async Task<IActionResult> Register(Owner owner)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingOwner = await _context.Owner.FirstOrDefaultAsync(x => x.Email == owner.Email);
            if (existingOwner != null)
                return BadRequest("Owner already exists with the same email address.");

            var login = new Login
            {
                Username = owner.Email,
                Password = owner.Password,
                Role = "Pending"
            };

            _context.Login.Add(login);
            await _context.SaveChangesAsync();

            owner.Lid = login.Lid;
            _context.Owner.Add(owner);
            await _context.SaveChangesAsync();

            return Ok("Owner registered successfully.");
        }

        // Login PG Owner
        [HttpPost("Login")]
        public async Task<IActionResult> Login(Login login)
        {
            var owner = await _context.Owner
                .FirstOrDefaultAsync(x => x.Email == login.Username && x.Password == login.Password);

            if (owner != null)
                return Ok(owner);
            return Unauthorized("Invalid credentials.");
        }

        // Create or Update PG listing
        [HttpPost("Pg")]
        public async Task<IActionResult> CreateOrUpdatePg(Pg pg)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (pg.Pgid > 0)
            {
                _context.Entry(pg).State = EntityState.Modified;
            }
            else
            {
                _context.Pg.Add(pg);
            }

            await _context.SaveChangesAsync();
            return Ok(pg);
        }

        // View PG listings for the Owner
        [HttpGet("MyPgs/{ownerId}")]
        public async Task<ActionResult<IEnumerable<Pg>>> GetMyPgs(int ownerId)
        {
            return await _context.Pg.Where(pg => pg.Oid == ownerId).ToListAsync();
        }

        // Delete PG listing
        [HttpDelete("Pg/{pgId}")]
        public async Task<IActionResult> DeletePg(int pgId)
        {
            var pg = await _context.Pg.FindAsync(pgId);
            if (pg == null)
                return NotFound("PG listing not found.");

            _context.Pg.Remove(pg);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Upload media for PG
        [HttpPost("Pg/{pgId}/Media")]
        public async Task<IActionResult> UploadMedia(int pgId, [FromForm] Media media)
        {
            if (media == null)
                return BadRequest("Media cannot be null.");

            media.Pid = pgId;
            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return Ok(media);
        }

        // View media for PG
        [HttpGet("Pg/{pgId}/Media")]
        public async Task<ActionResult<IEnumerable<Media>>> GetMediaForPg(int pgId)
        {
            return await _context.Media.Where(m => m.Pid == pgId).ToListAsync();
        }

        // Update pricing and availability
        [HttpPut("Pg/{pgId}/Room")]
        public async Task<IActionResult> UpdateRoomPricing(int pgId, Room room)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingRoom = await _context.Room.FirstOrDefaultAsync(r => r.Pid == pgId);
            if (existingRoom == null)
                return NotFound("Room not found.");

            existingRoom.Price = room.Price;
            existingRoom.Availability = room.Availability;
            existingRoom.Totalroom = room.Totalroom;
            existingRoom.Services = room.Services;
            existingRoom.Roomtype = room.Roomtype;
            existingRoom.Facility = room.Facility;

            _context.Entry(existingRoom).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingRoom);
        }

        // View reviews for a PG
        [HttpGet("Pg/{pgId}/Reviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsForPg(int pgId)
        {
            return await _context.Review.Where(r => r.Pid == pgId).ToListAsync();
        }

        // Respond to a review
        [HttpPut("Reviews/{reviewId}")]
        public async Task<IActionResult> RespondToReview(int reviewId, string response)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review == null)
                return NotFound("Review not found.");

            review.Reviewteaxt += $" - Response: {response}";
            _context.Entry(review).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Response added to the review.");
        }

        // Edit PG Owner profile
        [HttpPut("Profile/{ownerId}")]
        public async Task<IActionResult> UpdateProfile(int ownerId, Owner owner)
        {
            if (ownerId != owner.Oid)
                return BadRequest("Owner ID mismatch.");

            _context.Entry(owner).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // View PG Owner profile
        [HttpGet("Profile/{ownerId}")]
        public async Task<ActionResult<Owner>> GetProfile(int ownerId)
        {
            var owner = await _context.Owner.FindAsync(ownerId);
            if (owner == null)
                return NotFound("Owner not found.");

            return owner;
        }

        [HttpGet("Approved")]
        public async Task<IActionResult> GetOwnersByRole([FromQuery] string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return BadRequest("Role is required.");
            }

            var owners = await (from owner in _context.Owner
                                join login in _context.Login on owner.Lid equals login.Lid
                                where login.Role == role
                                select owner).ToListAsync();

            if (owners == null || !owners.Any())
            {
                return NotFound("No owners found for the specified role.");
            }

            return Ok(owners);
        }

        

    }
}


