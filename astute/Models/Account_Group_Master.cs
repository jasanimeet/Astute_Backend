using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Account_Group_Master
    {
        [Key]
        public int AC_GRP_CODE { get; set; }
        public string? AC_GRP_NAME { get; set; }
        public int? COMP_CODE { get; set; }
        public int? PARENT_GROUP { get; set; }
        public string? TRANS_TYPE { get; set; }
        public string? MAIN_GROUP { get; set; }
    }
}
