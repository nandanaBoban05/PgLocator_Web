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
    public class PgsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PgsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pgs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pg>>> GetPg()
        {
            return await _context.Pg.ToListAsync();
        }

        // GET: api/Pgs/5
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

        // PUT: api/Pgs/5
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

        // POST: api/Pgs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pg>> PostPg(Pg pg)
        {
            _context.Pg.Add(pg);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPg", new { id = pg.Pgid }, pg);
        }

        // DELETE: api/Pgs/5
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

        private bool PgExists(int id)
        {
            return _context.Pg.Any(e => e.Pgid == id);
        }

        //search
        [HttpGet("pgsearch")]
        public IActionResult SearchBooks(string? pgname = null, string? district = null, string? place = null)
        {
            var Pgserach = _context.Pg.AsQueryable();

            if (!string.IsNullOrEmpty(pgname))
            {
                Pgserach = Pgserach.Where(b => b.Pgname.ToLower().Contains(pgname.ToLower()));
            }

            if (!string.IsNullOrEmpty(district))
            {
                Pgserach = Pgserach.Where(b => b.District.ToLower().Contains(district.ToLower()));
            }

            if (!string.IsNullOrEmpty(place))
            {
                Pgserach = Pgserach.Where(b => b.Place.ToLower().Contains(place.ToLower()));
            }

            var filteredPg = Pgserach.ToList();

            if (!filteredPg.Any())
            {
                return NotFound("No books found matching the search criteria.");
            }

            return Ok(filteredPg);
        }


        ////seraches
      
        //[HttpGet("pgsearchs")]
        //public async Task<ActionResult<IEnumerable<Pg>>> SearchPg(string pg)
        //{
        //    var pgs = await _context.Pg
        //        .Where(b => b.Pgname.Contains(pg) || b.District.Contains(pg) || b.Place.Contains(pg))
        //        .ToListAsync();

        //    return Ok(pgs);
        //}
        //private bool PGExists(int id)
        //{
        //    return _context.Pg.Any(e => e.Pgid == id);
        //}
    }
}
