using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Exchange_Rate_Master
    {
        [Key]
        public int Exchange_Id { get; set; }
        public string? Trans_date { get; set; }
        public int? Currency_Id { get; set; }
        public string? Currency { get; set; }
        public decimal? Bank_Rate { get; set; }
        public decimal? Custom_Rate { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
