using System.Collections.Generic;

namespace astute.Models
{
    public class Transaction_Master_Model
    {
        public int Trans_Id { get; set; }
        public int Party_Code { get; set; }
        public int Due_Days { get; set; }
        public string Process { get; set; }
        public string Remarks { get; set; }
        public List<Transaction_Detail_Model> transaction_Detail_Model { get; set; } = new List<Transaction_Detail_Model>();

        public class Transaction_Detail_Model
        {
            public string Stock_Id { get; set; }
            public double Disc { get; set; }
            public double Amt { get; set; }

        }
    }

    public class Get_Transaction_Model
    { 
        public string? Stock_Id { get; set; }
        public string? Id { get; set; }
        public string? Sign { get; set; }
        public string? Value { get; set; }
        public int? Import_Id { get; set; }
        public string? Sheet_Name { get; set; }
    }
}
