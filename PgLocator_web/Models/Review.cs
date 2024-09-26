using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class Review
    {
        [Key]
        public int Rid { get; set; }
        [ForeignKey("Pg")]
        public int Pid { get; set; }
        [ForeignKey("User")]
        public int Uid { get; set; }
        public string Rating { get; set; }
        public string Reviewteaxt { get; set; }
        public DateTime Reviewdate { get; set; }
    }
}
