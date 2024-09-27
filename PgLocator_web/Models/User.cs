using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class User
    {
        [Key]
        public int Uid { get; set; }
        [ForeignKey("Login")]
        public int Lid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Dob { get; set; }
        public string Status { get; set; }
        public string Password { get; set; }
    }
}
