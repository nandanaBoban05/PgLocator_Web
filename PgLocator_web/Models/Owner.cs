using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class Owner
    {
        [Key]
        public int Oid { get; set; }
        [ForeignKey("Login")]
        public int Lid { get; set; }
        public string Ownername { get; set; }
        public string Email { get; set; }
        public DateOnly Dob { get; set; }
        public string Phone { get; set; }
        public string Whatsapp { get; set; }
        public string Chatlink { get; set; }
        public string Adress { get; set; }


    }
}
