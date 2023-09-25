using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Rapaport_Detail_Model
    {
        [Key]
        public int Rap_Det_Id { get; set; }
        public int Rap_Id { get; set; }
        public string Shape { get; set; }
        public decimal From_Cts { get; set; }
        public decimal To_Cts { get; set; }
        public string Color { get; set; }
        public string Clarity { get; set; }
        public decimal Rate { get; set; }
    }
}
