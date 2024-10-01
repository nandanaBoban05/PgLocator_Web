using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PgLocator_web.Data;
using PgLocator_web.Models;

namespace PgLocator_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "PGOwner, Admin")]
    public class PgOwnerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PgOwnerController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }



        // POST: api/PgOwner/UploadMedia
        [HttpPost("UploadMedia/{pgId}")]
        public async Task<IActionResult> UploadMedia(int pgId, [FromForm] IFormFile file)
        {
            // Check if PG exists
            var pg = await _context.Pg.FindAsync(pgId);
            if (pg == null)
            {
                return NotFound("PG not found");
            }

            // Check if file is not empty
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided");
            }

            // Create a folder path to store uploaded media
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique file name
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Save file to the server
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Save the media information in the database
            var media = new Media
            {
                Pid = pgId,
                Type = file.ContentType,  // For example, "image/jpeg" or "video/mp4"
                Url = "/uploads/" + uniqueFileName  // Path to access the file
            };

            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return Ok(new { mediaId = media.Mid, fileName = uniqueFileName, filePath = media.Url });
        }

        // GET: api/PgOwner/GetMediaByPg/5
        [HttpGet("GetMedia/{pgId}")]
        public async Task<ActionResult<IEnumerable<Media>>> GetMedia(int pgId)
        {
            var mediaList = await _context.Media
                .Where(m => m.Pid == pgId)
                .ToListAsync();

            if (mediaList == null || mediaList.Count == 0)
            {
                return NotFound("No media found for this PG");
            }

            return Ok(mediaList);
        }


        // EDIT MEDIA DETAILS: api/PgOwner/EditMedia/5
        [HttpPut("EditMedia/{mediaId}")]
        public async Task<IActionResult> EditMedia(int mediaId, Media updatedMedia)
        {
            var media = await _context.Media.FindAsync(mediaId);
            if (media == null)
            {
                return NotFound("Media not found");
            }

            // Update media details (excluding file upload)
            media.Type = updatedMedia.Type ?? media.Type;
            media.Url = updatedMedia.Url ?? media.Url;

            try
            {
                _context.Entry(media).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaExists(mediaId))
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

        // DELETE MEDIA: api/PgOwner/DeleteMedia/5
        [HttpDelete("DeleteMedia/{mediaId}")]
        public async Task<IActionResult> DeleteMedia(int mediaId)
        {
            var media = await _context.Media.FindAsync(mediaId);
            if (media == null)
            {
                return NotFound("Media not found");
            }

            // Delete the media file from the server
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, media.Url.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            // Remove media from the database
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // VIEW ALL MEDIA FOR A PG: api/PgOwner/GetMediaByPg/5 (Already Implemented)
        [HttpGet("GetMediaByPg/{pgId}")]
        public async Task<ActionResult<IEnumerable<Media>>> GetMediaByPg(int pgId)
        {
            var mediaList = await _context.Media
                .Where(m => m.Pid == pgId)
                .ToListAsync();

            if (mediaList == null || mediaList.Count == 0)
            {
                return NotFound("No media found for this PG");
            }

            return Ok(mediaList);
        }
        private bool MediaExists(int mediaId)
        {
            return _context.Media.Any(e => e.Mid == mediaId);
        }



        // GET: api/PgOwner
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pg>>> GetPg()
        {
            return await _context.Pg.ToListAsync();
        }

        // GET: api/PgOwner/5
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

        // PUT: api/PgOwner/5
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

        // POST: api/PgOwner
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pg>> PostPg(Pg pg)
        {
            _context.Pg.Add(pg);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPg", new { id = pg.Pgid }, pg);
        }

        // DELETE: api/PgOwner/5
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
    }
}
