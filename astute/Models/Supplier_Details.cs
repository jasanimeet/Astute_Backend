using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Supplier_Details
    {
        [Key]
        public int Party_Id { get; set; }
        public string Party_Name { get; set; }
        public Party_Api Party_Api { get; set; }
        public Party_FTP Party_FTP { get; set; }
        public Party_File Party_File { get; set; }
        public int? User_Id { get; set; }
        [NotMapped]
        public IList<Supplier_Column_Mapping> Supplier_Column_Mapping_List { get; set; } = new List<Supplier_Column_Mapping>();
        [NotMapped]
        public IList<Supplier_Value_Mapping> Supplier_Value_Mapping_List { get; set; } = new List<Supplier_Value_Mapping>();
    }
}
