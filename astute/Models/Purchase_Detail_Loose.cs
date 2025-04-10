namespace astute.Models
{
    public partial class Purchase_Detail_Loose
    {
        public int? Id { get; set; }
        public int? Trans_Id { get; set; }
        public int? Parcel_Type { get; set; }
        public int? Parcel_Name { get; set; }
        public float? Pcs { get; set; }
        public float? Cts { get; set; }
        public string? Unit { get; set; }
        public decimal? Rate_Unit { get; set; }
        public decimal? Value { get; set; }
    }
}