using PgLocator_web.Data;
using PgLocator_web.Models.Enitites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PgLocator_web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public ContactController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetContact()
        {
            return Ok(dbContext.Contact.ToList());
        }

        [HttpPost]
        public IActionResult AddContact(Contact contact)
        {
            var contactentity = new Contact()
            {
                Name = contact.Name,
                Email = contact.Email,
                Subject = contact.Subject,
                Message = contact.Message,
            };



            dbContext.Contact.Add(contactentity);
            dbContext.SaveChanges();

            return Ok(contactentity);

        }
    }
}
