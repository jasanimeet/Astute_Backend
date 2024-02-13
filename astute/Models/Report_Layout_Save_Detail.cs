using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Report_Layout_Save_Detail
    {
        [Key]
        public int Id { get; set; }
        public int? Report_Layout_Id { get; set; }
        public string? colId { get; set; }
        public int? width { get; set; }
        public bool? hide { get; set; }
        public string? pinned { get; set; }
        public string? sort { get; set; }
        public int? sortIndex { get; set; }
        public string? aggFunc { get; set; }
        public bool? rowGroup { get; set; }
        public int? rowGroupIndex { get; set; }
        public bool? pivot { get; set; }
        public int? pivotIndex { get; set; }
        public int? flex { get; set; }
    }
}
