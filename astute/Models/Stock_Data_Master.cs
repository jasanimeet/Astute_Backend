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
        public string? Upload_From { get; set; }
        public string? Stock_Type { get; set; }
        [NotMapped]
        public IList<Stock_Data> Stock_Data_List { get; set; } = new List<Stock_Data>();
    }

    public partial class Stock_Data_Master_Schedular
    {
        [Key]
        public int Stock_Data_Id { get; set; }
        public int? Supplier_Id { get; set; }
        public string? Upload_Method { get; set; }
        public string? Upload_Type { get; set; }
        public string? Error_Message { get; set; }
        public string? Upload_From { get; set; }
        public string? Stock_Type { get; set; }
        [NotMapped]
        public IList<Stock_Data_Schedular> Stock_Data_List { get; set; } = new List<Stock_Data_Schedular>();
    }

    public partial class Stock_Data_Master_Excel
    {
        public string? Supplier_Id { get; set; }
        public object? Supplier_Stock_Data { get; set; }
    }
}
