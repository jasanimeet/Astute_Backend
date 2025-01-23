using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Parcel_Ref_Master
    {
        [Key]
        public int? Parcel_Ref_Id { get; set; }
        public int? Parcel_Id { get; set; }
        public string? Parcel_Name { get; set; }
        public string? Unit { get; set; }
        public bool? Status { get; set; }

        //[NotMapped]
        //public bool IsForceInsert { get; set; }

    }
}
