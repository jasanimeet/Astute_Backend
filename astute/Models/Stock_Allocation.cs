using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Stock_Allocation
    {
        [Key]
        public int? Id { get; set; }
        public int? Parcel_Id { get; set; }
        public int? Parcel_Ref_Id { get; set; }
        public int? Ac_Grp_Code { get; set; }
        public string? Trans_Type { get; set; }
        public string? PCS { get; set; }
        public string? CTS { get; set; }
        [JsonPropertyName("Amount_In_US($)")]
        public string? Amount_In_US { get; set; }
        public int? Company_Id { get; set; }
        public int? Year_Id { get; set; }
    }
}
