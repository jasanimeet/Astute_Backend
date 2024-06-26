using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Import_Master
    {
        [Key]
        public int Import_Id { get; set; }
        public string Format_Name { get; set; }
        public int Type { get; set; }
        public string? Created_Date { get; set; }
        public string? Created_Time { get; set; }
        public string? Created_By { get; set; }
        public string? Updated_Date { get; set; }
        public string? Updated_Time { get; set; }
        public string? Updated_By { get; set; }
    }
}
