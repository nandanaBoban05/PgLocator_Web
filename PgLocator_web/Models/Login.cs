using System.ComponentModel.DataAnnotations;

namespace PgLocator_web.Models
{
    public class Login
    {
        [Key]
        public int Lid { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
