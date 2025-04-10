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
            Usert_Type = employee_Master.User_Type;
            Is_Secretary = employee_Master.Is_Secretary;
            Confirm_Purchase = employee_Master.Confirm_Purchase;
        }
        public int Id { get; set; }
        public string? Username { get; set; }
        public bool? Is_Admin { get; set; }
        public bool? Is_Secretary { get; set; }
        public bool? Confirm_Purchase { get; set; }
        public string Token { get; set; }
        public string Usert_Type { get; set; }
    }
}
