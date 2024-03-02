using System.Collections.Generic;

namespace astute.Models
{
    public class Supplier_API_FTP_File_Details
    {
        public Party_Api Party_Api { get; set; }
        public Party_FTP Party_FTP { get; set; }
        public Party_File Party_File { get; set; }
        public string? Upload_Type { get; set; }
        public IList<Supplier_Column_Mapping> Supplier_Column_Mapping_List { get; set; } = new List<Supplier_Column_Mapping>();
    }
}
