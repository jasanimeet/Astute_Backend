using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Layout_Column
    {
        [Key]
        public int Layout_Column_Id { get; set; }
        public int LayoutDet_ID { get; set; }
        public string Column_Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public bool Status { get; set; }
    }
}
