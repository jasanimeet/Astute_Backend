using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    [Keyless]
    public class Report_Detail
    {
        public int Id { get; set; }
        public int Rm_Id { get; set; }
        public string Display_Name { get; set; }
        public string Column_Type { get; set; }
        public int Col_Id { get; set; }
        public string Order_By { get; set; }
        public int Short_No { get; set; }
        public string Display_Type { get; set; }
        public int Width { get; set; }
        public string Column_Format { get; set; }
        public string Alignment { get; set; }
        public string Fore_Colour { get; set; }
        public string Back_Colour { get; set; }
        public bool IsBold { get; set; }

    }
}
