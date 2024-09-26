using System.ComponentModel.DataAnnotations.Schema;

namespace PgLocator_web.Models
{
    public class Room
    {
        public int Rid { get; set; }
        [ForeignKey("Pg")]
        public int Pid { get; set; }
        public int Price { get; set; }
        public int Deposit { get; set; }
        public string Services { get; set; }
        public string Roomtype { get; set; }
        public string Facility { get; set; }
        public int Totalroom { get;set; }
        public int Availability { get; set; }





    }
}
