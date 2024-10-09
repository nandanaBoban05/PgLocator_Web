using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PgLocator_web.Data;
using PgLocator_web.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PgLocator_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MediaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Media
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Media>>> GetMedia()
        {
            return await _context.Media.ToListAsync();
        }

        // GET: api/Media/ByPgId/5
        [HttpGet("ByPgId/{pgId}")]
        public async Task<ActionResult<IEnumerable<Media>>> GetMediaByPgId(int pgId)
        {
            var media = await _context.Media.Where(m => m.Pid == pgId).ToListAsync();
            if (media == null || !media.Any())
            {
                return NotFound();
            }
            return media;
        }

        // GET: api/Media/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Media>> GetMedia(int id)
        {
            var media = await _context.Media.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }
            return media;
        }

        // POST: api/Media/UploadMedia (save file as byte array)
        [HttpPost("UploadMedia")]
        public async Task<IActionResult> UploadMedia(int pgId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            // Create a new media record and save the byte array in FileData
            var media = new Media
            {
                Pid = pgId,
                Type = file.ContentType,  // Store the media type (image, video, etc.)
                FileData = fileBytes // Save the byte array
            };

            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return Ok(media);
        }

        // PUT: api/Media/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedia(int id, Media media)
        {
            if (id != media.Mid)
            {
                return BadRequest();
            }

            _context.Entry(media).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MediaExists(id))
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

        // DELETE: api/Media/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedia(int id)
        {
            var media = await _context.Media.FindAsync(id);
            if (media == null)
            {
                return NotFound();
            }

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MediaExists(int id)
        {
            return _context.Media.Any(e => e.Mid == id);
        }
    }
}
