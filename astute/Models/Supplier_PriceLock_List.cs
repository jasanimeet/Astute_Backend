using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Supplier_PriceLock_List
    {
        [Key]
        public int Party_Id { get; set; }
        public string? Party_Name { get; set; }
        public bool? Price_Lock { get; set; }
        public string Stage { get; set; }
    }
}
