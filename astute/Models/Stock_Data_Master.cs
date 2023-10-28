using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Stock_Data_Master
    {
        [Key]
        public int Stock_Data_Id { get; set; }
        public int? Supplier_Id { get; set; }
        public string? Upload_Method { get; set; }
    }
}
