namespace astute.Models
{
    public class Account_Master
    {
        public int? Account_Id { get; set; }
        public string? Account_Name { get; set; }
        public string Group { get; set; }
        public string? Sub_Group { get; set; }
        public int? Main_Company { get; set; }
        public string? Purchase_Expence { get; set; }
        public string? Sales_Expence { get; set; }
        public bool? Is_Party { get; set; }
        public int? User_Id { get; set; }
        public decimal? Default_Per { get; set; }
    }
}
