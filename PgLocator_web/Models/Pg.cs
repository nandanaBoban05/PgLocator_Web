using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class Pg
    {
        [Key]
        public int Pgid { get; set; }
        [ForeignKey("Owner")]
        public int Oid { get; set; }
        public string Pgname { get; set; }
        public string Description { get; set; }
        public string Adress { get; set; }
        public int Pin { get; set; }
        public string Gender { get; set; }
        public string Image { get; set; }
        public string Amentities { get; set; }
        public bool Foodavailable { get; set; }
        public string Meal { get; set; }
        public string Status { get; set; }
        public string Rules { get; set; }
        public string District { get; set; }
        public string Place { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }   




    }
}
