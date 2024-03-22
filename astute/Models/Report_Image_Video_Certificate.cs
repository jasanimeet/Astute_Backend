using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Report_Image_Video_Certificate
    {
        [Key]
        public int Id { get; set; }
        public string Stock_Id { get; set; }
        public string Url { get; set; }

    }
}
