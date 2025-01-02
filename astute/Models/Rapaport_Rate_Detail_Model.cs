using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Rapaport_Rate_Detail_Model
    {
        [Key]
        public int? Rap_Id { get; set; }
        public string? Certificate_No { get; set; }
        public int? Shape { get; set; }
        public decimal? Cts { get; set; }
        public int? Color { get; set; }
        public int? Clarity { get; set; }
        public decimal? Rap_Rate { get; set; }
        public decimal? Rap_Amt { get; set; }
    }
}
