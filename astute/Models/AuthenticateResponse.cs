using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering.Implementations;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;

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
        }
        public int Id { get; set; }
        public string? Username { get; set; }
        public bool? Is_Admin { get; set; }
        public string Token { get; set; }
    }
}
