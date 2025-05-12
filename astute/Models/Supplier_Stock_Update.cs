namespace astute.Models
{
    public class Supplier_Stock_Update
    {

        public int Supplier_Id { get; set; }
        public int? Stock_Data_Id { get; set; }
        public string? Start_Time { get; set; }
        public string? Supplier_Response_Time { get; set; }
        public string? End_Time { get; set; }
        public string? Upload_Method { get; set; }
        public string? Upload_Type { get; set; }
        public string? Upload_Status { get; set; }
        public string? Stock_Type { get; set; }
    }
}
