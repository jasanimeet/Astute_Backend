using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Temp_Layout_Detail
    {
        [Key]
        public int Layout_Detail_Id { get; set; }
        public int? Layout_Id { get; set; }
        public string? dataField { get; set; }
        public string? dataType { get; set; }
        public string? filterValues { get; set; }
        public string? name { get; set; }
        public int? sortIndex { get; set; }
        public string? sortOrder { get; set; }
        public bool? visible { get; set; }
        public int? visibleIndex { get; set; }
        public int? width { get; set; }
    }
}
