using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Rapaport_Detail
    {
        [Key]
        public int Rap_Det_Id { get; set; }
        public int Rap_Id { get; set; }
        public decimal From_Cts { get; set; }
        public decimal To_Cts { get; set; }
        public int Color_Id { get; set; }
        public string Color { get; set; }
        public int Clarity_Id { get; set; }
        public string Clarity { get; set; }
        public decimal Rate { get; set; }
        public string Shape { get; set; }
    }
}
