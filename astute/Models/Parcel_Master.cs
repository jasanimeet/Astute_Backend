using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Parcel_Master
    {
        [Key]
        public int? Parcel_Id { get; set; }
        public string? Parcel_Name { get; set; }
        public int? Cat_Val_Id { get; set; }
        public string? Unit { get; set; }
        public int? Pointer { get; set; }
        public string? Shape { get; set; }
        public string? Color { get; set; }
        public string? Clarity { get; set; }
        public bool? Status { get; set; }

        [NotMapped]
        public bool IsForceInsert { get; set; }

    }
}
