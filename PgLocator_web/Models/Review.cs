using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class Review
    {
        [Key]
        public int Rid { get; set; }
        
        [ForeignKey("Pg")]
        public int Pgid { get; set; }
        
        [ForeignKey("User")]
        public int Uid { get; set; }
        public int Rating { get; set; }
        public string Reviewtext { get; set; }
        public DateTime Reviewdate { get; set; }
        public string? Response { get; set; }
        public bool? IsReported { get; set; }  
        public bool? ReportedToAdmin { get; set; }
    }
}
