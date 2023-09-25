using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Holiday_Master
    {
        [Key]
        public int Holiday_Id { get; set; }
        public DateTime Date { get; set; }
        public string Start_Time { get; set; }
        public string End_Time { get; set;}
        public bool Holiday_Flag { get; set; }
        public string Description { get; set; }
    }
}
