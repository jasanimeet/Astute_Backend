namespace astute.Models
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse(Employee_Master employee_Master, string token)
        {
            Id = employee_Master.Employee_Id;
            Username = employee_Master.User_Name;
            Is_Admin = employee_Master.Is_Admin;
            Token = token;
            User_Type = employee_Master.User_Type;
            Is_Secretary = employee_Master.Is_Secretary;
            Confirm_Purchase = employee_Master.Confirm_Purchase;
            Upcoming_Approved = employee_Master.Upcoming_Approved;
            Repricing_Approved = employee_Master.Repricing_Approved;
            Upcoming_Approval = employee_Master.Upcoming_Approval;
            Repricing_Approval = employee_Master.Repricing_Approval;
            Middle_Name = employee_Master.Middle_Name;
            Buyer_Code = employee_Master.Fortune_Id;
        }
        public int Id { get; set; }
        public string? Username { get; set; }
        public bool? Is_Admin { get; set; }
        public bool? Is_Secretary { get; set; }
        public bool? Confirm_Purchase { get; set; }
        public bool? Upcoming_Approved { get; set; }
        public bool? Repricing_Approved { get; set; }
        public bool? Upcoming_Approval { get; set; }
        public bool? Repricing_Approval { get; set; }
        public string Token { get; set; }
        public string User_Type { get; set; }
        public string Middle_Name { get; set; }
        public int? Buyer_Code { get; set; }
    }
}
