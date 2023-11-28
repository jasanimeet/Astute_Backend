using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Supplier_Pricing_Key_To_Symbol
    {
        [Key]
        public int Id { get; set; }
        public int? Supplier_Pricing_Id { get; set; }
        public int Cat_Val_Id { get; set; }
        public bool? Symbol_Status { get; set; }
    }
}
