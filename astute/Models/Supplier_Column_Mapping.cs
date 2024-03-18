using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Supplier_Column_Mapping
    {
        [Key]
        public int? Col_Id { get; set; }
        public string? Col_Name { get; set; }
        public string? Supp_Col_Name { get; set; }
        public string? Column_Type { get; set; }
        public int? Supp_Col_Id { get; set; }
        public int? Supp_Id { get; set; }
        public string? Column_Synonym { get; set; }
        public string? Display_Name { get; set; }
        public string? Display_Type { get; set; }
        public bool? Is_Split { get; set; }
        public string? Separator { get; set; }
    }
}
