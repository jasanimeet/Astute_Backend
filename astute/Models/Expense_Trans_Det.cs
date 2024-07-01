using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Expense_Trans_Det
    {
        [Key]
        public int Expense_Trans_Det_Id { get; set; }
        public int Account_Master_Id { get; set; }
        public string Sign { get; set; }
        public decimal Percentage { get; set; }
        public decimal Amount { get; set; }
        public int Trans_Id { get; set; }
        public string Trans_Type { get; set; }
}
}
