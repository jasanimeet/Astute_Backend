using Org.BouncyCastle.Bcpg.OpenPgp;

namespace astute.Models
{
    public partial class Stock_Email_Model
    {
        public string? Stock_Upload_Type { get; set; }
        public string? Supp_Ref_No { get; set; }
        public string? To_Email { get; set; }
        public string? Remarks { get; set; }
    }
}
