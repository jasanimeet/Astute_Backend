using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Import_Detail
    {
        [Key]
        public int Import_Det_Id { get; set; }
        public int Import_Id { get; set; }
        public int Column_Name { get; set; }
        public int Excel_Column_No { get; set; }
        public bool Required { get; set; }
    }
}
