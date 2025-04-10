namespace astute.Models
{
    public partial class Purchase_Master_Search_Model
    {
        public string? From_Date { get; set; }
        public string? To_Date { get; set; }
        public string? Doc_Type { get; set; }
        public string? Stock_Status { get; set; }
        public string? Stock_Certificate_No { get; set; }
        public int? Company_Id { get; set; }
    }
}