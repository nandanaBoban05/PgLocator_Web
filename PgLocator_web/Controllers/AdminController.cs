using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PgLocator_web.Data;
using PgLocator_web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace PgLocator_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }


        // Approve or reject PG Owner
        [HttpPost]
        [Route("ApproveRejectPgOwner")]
        public async Task<IActionResult> ApproveRejectPgOwner(int ownerId, string action)
        {
            var owner = await _context.Owner.FindAsync(ownerId);
            if (owner == null)
            {
                return NotFound("PG Owner not found");
            }

            var login = await _context.Login.FindAsync(owner.Lid); 
            if (login == null)
            {
                return NotFound("Login entry not found for this PG owner");
            }

            // Admin action: Approve or Reject
            if (action.ToLower() == "approve")
            {
                login.Role = "Approved";
            }
            else if (action.ToLower() == "reject")
            {
                login.Role = "Rejected";
            }
            else
            {
                return BadRequest("Invalid action. Use 'approve' or 'reject'");
            }

            _context.Entry(login).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"PG Owner {action}d successfully");
        }

        // View all approved PG Owners
        [HttpGet]
        [Route("ViewApprovedPgOwners")]
        public async Task<IActionResult> ViewApprovedPgOwners()
        {
            var approvedOwners = await _context.Owner
                .Where(o => _context.Login.Any(l => l.Lid == o.Lid && l.Role == "Approved"))
                .ToListAsync();

            return Ok(approvedOwners);
        }

        // Approve or reject PG Listing
        [HttpPost]
        [Route("ApproveRejectPg")]
        public async Task<IActionResult> ApproveRejectPg(int pgId, string action)
        {
            var pg = await _context.Pg.FindAsync(pgId);
            if (pg == null)
            {
                return NotFound("PG Listing not found");
            }

            // Admin action: Approve or Reject
            if (action.ToLower() == "approve")
            {
                pg.Status = "Approved";
            }
            else if (action.ToLower() == "reject")
            {
                pg.Status = "Rejected";
            }
            else
            {
                return BadRequest("Invalid action. Use 'approve' or 'reject'");
            }

            _context.Entry(pg).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok($"PG Listing {action}d successfully");
        }

        // View all approved PG Listings
        [HttpGet]
        [Route("ViewApprovedPg")]
        public async Task<IActionResult> ViewApprovedPg()
        {
            var approvedPgs = await _context.Pg
                .Where(pg => pg.Status == "Approved")
                .ToListAsync();

            return Ok(approvedPgs);
        }
    }
}
