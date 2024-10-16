using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PgLocator_web.Data;
using PgLocator_web.Models;

namespace PgLocator_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public MediaController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // Upload media
        [HttpPost("upload/{pgId}")]
        public async Task<IActionResult> UploadMedia(int pgId, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var filePath = Path.Combine(_environment.ContentRootPath, "uploads", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var media = new Media
            {
                Pgid = pgId,
                FilePath = filePath
            };

            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return Ok(media);
        }

        // Get media by PG ID
        [HttpGet("{pgId}")]
        public async Task<IActionResult> GetMediaByPgId(int pgId)
        {
            var mediaList = await _context.Media
                .Where(m => m.Pgid == pgId)
                .ToListAsync();

            return Ok(mediaList);
        }

        // Delete media
        [HttpDelete("{mediaId}")]
        public async Task<IActionResult> DeleteMedia(int mediaId)
        {
            var media = await _context.Media.FindAsync(mediaId);
            if (media == null)
                return NotFound();

            // Delete the file from the server
            if (System.IO.File.Exists(media.FilePath))
            {
                System.IO.File.Delete(media.FilePath);
            }

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    
}
}
