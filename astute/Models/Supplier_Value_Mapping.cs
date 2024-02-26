using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Supplier_Value_Mapping
    {
        [Key]
        public int Our_Cat_Val_Id { get; set; }
        public string? Cat_Value { get; set; }
        public int? Sup_Id { get; set; }
        public string? Supp_Cat_Name { get; set; }
        public int? Cat_val_Id { get; set; }
        public int? Col_Id { get; set; }
        public bool? Status { get; set; }
    }
}
