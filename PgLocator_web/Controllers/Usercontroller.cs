using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    public class Usercontroller : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public Usercontroller(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var objUser = _context.User.FirstOrDefault(x => x.Email == user.Email);
            if (objUser == null)
            {

                _context.User.Add(new User
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role,
                    Email = user.Email,
                    Phone = user.Phone,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    Whatsapp = user.Whatsapp,
                    Chatlink = user.Chatlink,
                    Address = user.Address,
                    Status = user.Status,
                    Password = user.Password,
                });
                _context.SaveChanges();
                return Ok("User Registered Successfully");
            }
            else
            {
                return BadRequest("User already exist");
            }

        }

        //[HttpPost]
        //[Route("Login")]
        //public IActionResult Login(User user) {
        //    var User = _context.User.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
        //    if (user != null)
        //    {
        //        return Ok(user);
        //    }
        //    return BadRequest();
        //}
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginModel)
        {
            var user = await _context.User
                .FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == loginModel.Password &&u.Status==loginModel.Status);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid username or password" });
            }

            // Return success message and userId
            return Ok(new
            {
                message = "Login successful",
                userId = user.Uid  // Assuming the user's ID is UId
            });
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers() {
            return Ok(_context.User.ToList());
        }

        [HttpGet]
        [Route("GetUser")]
        public IActionResult GetUsers(int id)
        {
            var user = _context.User.FirstOrDefault(x => x.Uid == id);
            if(user != null) 
               return Ok();
            else
                return NoContent();
        }

    }
}
