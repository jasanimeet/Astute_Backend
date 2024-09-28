namespace astute.Models
{
    public class Trans_Model
    {
        public int Id { get; set; }
        public int Trans_Type { get; set; }
        public string? Prefix { get; set; }
        public string? Voucher_No { get; set; }
        public bool Post_Prefix { get; set; }
        public bool Year_Reset { get; set; }
    }
}
