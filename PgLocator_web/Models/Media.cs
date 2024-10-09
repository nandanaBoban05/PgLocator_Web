using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class Media
    {
        [Key]
        public int Mid { get; set; }
        [ForeignKey("Pg")]
        public int Pid { get; set; }
        public string Type { get; set; }
        public byte[] FileData { get; set; }

    }
}
