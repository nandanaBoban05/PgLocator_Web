using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class Media
    {
        [Key]
        public int MediaId { get; set; }

        [ForeignKey("Pg")]
        public int Pgid { get; set; }  

        public string FilePath { get; set; } 

        public virtual Pg Pg { get; set; } 
    }
}
