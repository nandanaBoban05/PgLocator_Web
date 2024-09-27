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

        // Create PG List
        [HttpPost("CreatePg")]
        public async Task<IActionResult> CreatePg([FromBody] Pg pg)
        {
            if (pg == null)
                return BadRequest("PG data is required.");

            _context.Pg.Add(pg);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreatePg), new { id = pg.Pgid }, pg);
        }

        // View PG List
        [HttpGet("ViewPgList")]
        public async Task<IActionResult> ViewPgList(int ownerId)
        {
            var pgList = await _context.Pg.Where(p => p.Oid == ownerId).ToListAsync();
            return Ok(pgList);
        }

        // Update PG Listing
        [HttpPut("UpdatePg/{id}")]
        public async Task<IActionResult> UpdatePg(int id, [FromBody] Pg pg)
        {
            if (pg == null)
                return BadRequest("PG data is required.");

            var existingPg = await _context.Pg.FindAsync(id);
            if (existingPg == null)
                return NotFound("PG listing not found.");

            existingPg.Pgname = pg.Pgname;
            existingPg.Description = pg.Description;
            existingPg.Adress = pg.Adress;
            existingPg.Pin = pg.Pin;
            existingPg.Gender = pg.Gender;
            existingPg.Image = pg.Image;
            existingPg.Amentities = pg.Amentities;
            existingPg.Foodavailable = pg.Foodavailable;
            existingPg.Meal = pg.Meal;
            existingPg.Status = pg.Status;
            existingPg.Rules = pg.Rules;
            existingPg.District = pg.District;
            existingPg.Place = pg.Place;
            existingPg.Latitude = pg.Latitude;
            existingPg.Longitude = pg.Longitude;

            _context.Entry(existingPg).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete PG Listing
        [HttpDelete("DeletePg/{id}")]
        public async Task<IActionResult> DeletePg(int id)
        {
            var pg = await _context.Pg.FindAsync(id);
            if (pg == null)
                return NotFound("PG listing not found.");

            _context.Pg.Remove(pg);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        // Add Media for PG
        [HttpPost("AddMedia")]
        public async Task<IActionResult> AddMedia([FromBody] Media media)
        {
            if (media == null)
                return BadRequest("Media data is required.");

            _context.Media.Add(media);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(AddMedia), new { id = media.Mid }, media);
        }

        // View Media for PG
        [HttpGet("ViewMedia/{pgId}")]
        public async Task<IActionResult> ViewMedia(int pgId)
        {
            var mediaList = await _context.Media.Where(m => m.Pid == pgId).ToListAsync();
            return Ok(mediaList);
        }

        // Update Media
        [HttpPut("UpdateMedia/{id}")]
        public async Task<IActionResult> UpdateMedia(int id, [FromBody] Media media)
        {
            if (media == null)
                return BadRequest("Media data is required.");

            var existingMedia = await _context.Media.FindAsync(id);
            if (existingMedia == null)
                return NotFound("Media not found.");

            existingMedia.Type = media.Type;
            existingMedia.Url = media.Url;

            _context.Entry(existingMedia).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete Media
        [HttpDelete("DeleteMedia/{id}")]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            var media = await _context.Media.FindAsync(id);
            if (media == null)
                return NotFound("Media not found.");

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // 

        // Add Room Pricing
        [HttpPost("AddRoomPricing")]
        public async Task<IActionResult> AddRoomPricing([FromBody] Room room)
        {
            if (room == null)
                return BadRequest("Room data is required.");

            _context.Room.Add(room);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(AddRoomPricing), new { id = room.Rid }, room);
        }

        // View Room Pricing
        [HttpGet("ViewRoomPricing/{pgId}")]
        public async Task<IActionResult> ViewRoomPricing(int pgId)
        {
            var roomPricingList = await _context.Room.Where(r => r.Pid == pgId).ToListAsync();
            return Ok(roomPricingList);
        }

        // Update Room Pricing
        [HttpPut("UpdateRoomPricing/{id}")]
        public async Task<IActionResult> UpdateRoomPricing(int id, [FromBody] Room room)
        {
            if (room == null)
                return BadRequest("Room data is required.");

            var existingRoom = await _context.Room.FindAsync(id);
            if (existingRoom == null)
                return NotFound("Room not found.");

            existingRoom.Price = room.Price;
            existingRoom.Deposit = room.Deposit;
            existingRoom.Services = room.Services;
            existingRoom.Roomtype = room.Roomtype;
            existingRoom.Facility = room.Facility;
            existingRoom.Totalroom = room.Totalroom;
            existingRoom.Availability = room.Availability;

            _context.Entry(existingRoom).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // Delete Room Pricing
        [HttpDelete("DeleteRoomPricing/{id}")]
        public async Task<IActionResult> DeleteRoomPricing(int id)
        {
            var room = await _context.Room.FindAsync(id);
            if (room == null)
                return NotFound("Room not found.");

            _context.Room.Remove(room);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // View and Respond to Reviews
        [HttpGet("ViewReviews/{pgId}")]
        public async Task<IActionResult> ViewReviews(int pgId)
        {
            var reviews = await _context.Review.Where(r => r.Pid == pgId).ToListAsync();
            return Ok(reviews);
        }

        [HttpPost("RespondToReview/{reviewId}")]
        public async Task<IActionResult> RespondToReview(int reviewId, [FromBody] string response)
        {
            var review = await _context.Review.FindAsync(reviewId);
            if (review == null)
                return NotFound("Review not found.");

            review.Reviewteaxt += "\nResponse: " + response; // Append response to review text
            await _context.SaveChangesAsync();
            return NoContent();
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
    }
}
