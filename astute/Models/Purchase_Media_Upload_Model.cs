namespace astute.Models
{
    public partial class Purchase_Media_Upload_Model
    {
        public int? Id { get; set; }
        public string? Sunrise_Stock_Id { get; set; }
        public int? Image_Status { get; set; }
        public int? Video_Status { get; set; }
        public int? Certificate_Status { get; set; }
        public bool? IsFortune { get; set; }
        public string? NewImageUrl { get; set; }
        public string? NewVideoUrl { get; set; }
    }
}