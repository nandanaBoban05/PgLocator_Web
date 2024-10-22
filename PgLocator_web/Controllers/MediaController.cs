using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PgLocator_web.Data;
using PgLocator_web.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

            // Check if directory exists, if not, create it
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"pg{pgId}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

            // Save the file path or URL in the database if needed
            var media = new Media
            {
                Pgid = pgId,
                FilePath = fileUrl // Save the file URL
            };

            _context.Media.Add(media);
            await _context.SaveChangesAsync();

            return Ok(new { fileUrl });
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
            var fullFilePath = Path.Combine(_environment.WebRootPath, media.FilePath);
            if (System.IO.File.Exists(fullFilePath))
            {
                System.IO.File.Delete(fullFilePath);
            }

            // Remove the media record from the database
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
