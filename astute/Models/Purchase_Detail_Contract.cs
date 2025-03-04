namespace astute.Models
{
    public class Purchase_Detail_Contract_List
    {
        public object? Purchase_Detail_Contract { get; set; }
    }

    public partial class Purchase_Detail_Contract
    {
        public int? Id { get; set; }
        public bool? Contract { get; set; }
    }
}
