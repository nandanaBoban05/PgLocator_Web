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
    public class PgController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PgController(ApplicationDbContext context)
        {
            _context = context;
        }


        // POST: api/pg/register
    [HttpPost("register")]
    public IActionResult RegisterPG([FromBody] Pg pg)
            {
                if (pg == null)
                {
                    return BadRequest("PG data is null.");
                }

                try
                {
                    pg.Status = "Pending";  // Initial status as 'pending'
                    _context.Pg.Add(pg);
                    _context.SaveChanges();

                    return Ok(new { message = "PG added and awaiting approval." });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }


        // GET: api/pg/owner/{ownerId}/approved
        [HttpGet("owner/{ownerId}/approved")]
        public IActionResult GetApprovedPGsForOwner(int ownerId)
        {
            try
            {
                var approvedPGs = _context.Pg
                                          .Where(pg => pg.Uid == ownerId && pg.Status == "Approved")
                                          .ToList();

                if (!approvedPGs.Any())
                {
                    return NotFound($"No approved PGs found for owner with ID {ownerId}.");
                }

                return Ok(approvedPGs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }




        // GET: api/Pg
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pg>>> GetPg()
        {
            return await _context.Pg.ToListAsync();
        }

        // GET: api/Pg/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pg>> GetPg(int id)
        {
            var pg = await _context.Pg.FindAsync(id);

            if (pg == null)
            {
                return NotFound();
            }

            return pg;
        }
        // PUT: api/Pg/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPg(int id, Pg pg)
        {
            if (id != pg.Pgid)
            {
                return BadRequest();
            }

            _context.Entry(pg).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PgExists(id))
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

        // POST: api/Pg
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pg>> PostPg(Pg pg)
        {
            _context.Pg.Add(pg);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPg", new { id = pg.Pgid }, pg);
        }

        // DELETE: api/Pg/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePg(int id)
        {
            var pg = await _context.Pg.FindAsync(id);
            if (pg == null)
            {
                return NotFound();
            }

            _context.Pg.Remove(pg);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("search")]
        public IActionResult SearchPgs(string? district = null, string? city = null)
        {
            // Start with all PGs
            var pgs = _context.Pg.AsQueryable();

            // Filter by district if provided
            if (!string.IsNullOrEmpty(district))
            {
                pgs = pgs.Where(pg => pg.District.ToLower().Contains(district.ToLower()));
            }

            // Filter by city if provided
            if (!string.IsNullOrEmpty(city))
            {
                pgs = pgs.Where(pg => pg.City.ToLower().Contains(city.ToLower()));
            }

            // Convert filtered results to a list
            var filteredPgs = pgs.ToList();

            // If no PGs match, return a 404 NotFound response
            if (!filteredPgs.Any())
            {
                return NotFound("No PGs found matching the search criteria.");
            }

            // Return the filtered list of PGs
            return Ok(filteredPgs);
        }
        private bool PgExists(int id)
        {
            return _context.Pg.Any(e => e.Pgid == id);
        }
    }
}
