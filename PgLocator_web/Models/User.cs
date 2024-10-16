using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class User
    {
        [Key]
        public int Uid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Role { get; set; }

        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string Dob { get; set; }
        public string? Whatsapp { get; set; }
        public string? Chatlink { get; set; }
        public string? Address { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
    }
}
