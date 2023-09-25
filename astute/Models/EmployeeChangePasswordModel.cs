namespace astute.Models
{
    public partial class EmployeeChangePasswordModel
    {
        public int Employee_Id { get; set; }
        public string Old_Password { get; set; }
        public string New_Password { get; set;}
    }
}
