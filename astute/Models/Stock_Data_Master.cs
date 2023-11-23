using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Stock_Data_Master
    {
        [Key]
        public int Stock_Data_Id { get; set; }
        public int? Supplier_Id { get; set; }
        public string? Upload_Method { get; set; }
        public string? Upload_Type { get; set; }
        [NotMapped]
        public IList<Stock_Data> Stock_Data_List { get; set; } = new List<Stock_Data>();
    }
}
