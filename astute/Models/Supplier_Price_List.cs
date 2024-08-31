using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Supplier_Price_List
    {
        [Key]
        public int Party_Id { get; set; }
        public string? Party_Name { get; set; }
        public bool? Disc_0 { get; set; }
        public bool? No_Stock { get; set; }
    }
}
