using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Report_Detail
    {
        [Key]
        public int Id { get; set; }
        public int Rm_Id { get; set; }
        public string Column_Type { get; set; }
        public int Col_Id { get; set; }
        public int Order_No { get; set; }
        public string Display_Type { get; set; }
        public int Width { get; set; }
        public string Column_Format { get; set; }
        public string Alignment { get; set; }
        public string Fore_Colour { get; set; }
        public string Back_Colour { get; set; }

    }
}
