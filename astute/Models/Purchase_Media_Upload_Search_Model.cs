namespace astute.Models
{
    public partial class Purchase_Media_Upload_Search_Model
    {
        public string? From_Date { get; set; }
        public string? To_Date { get; set; }
        public int? Image_Status { get; set; }
        public int? Video_Status { get; set; }
        public int? Certificate_Status { get; set; }
        public bool? PreSold { get; set; }
    }
}