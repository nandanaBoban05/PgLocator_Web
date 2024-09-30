using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class Pg
    {
        [Key]
        public int Pgid { get; set; }
        [ForeignKey("User")]
        public int Uid { get; set; }
        public string Pgname { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Pin { get; set; }
        public string Gender_perference { get; set; }
        public string Amentities { get; set; }
        public bool Foodavailable { get; set; }
        public string Meal { get; set; }
        public string Status { get; set; }
        public string Rules { get; set; }



    }
}
