namespace astute.Models
{
    public class Purchase_Approval
    {
        public string Trans_Id { get; set; }
        public bool? Is_Upcoming_Approval { get; set; }
        public bool? Is_Repricing_Approval { get; set; }
    }
}
