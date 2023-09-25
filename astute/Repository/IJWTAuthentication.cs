using astute.Models;
using System.Threading.Tasks;

namespace astute.Repository
{
    public interface IJWTAuthentication
    {
        string Generate_Jwt_Token(Employee_Master employee_Master);
        int? Validate_Jwt_Token(string? token);
        Task<int> Insert_Update_Employee_JWT_Token(Employee_JWT_Token employee_JWT_Token);
        Task<Employee_JWT_Token> Get_Employee_JWT_Token(int employee_Id);
        Task<int> Delete_Employee_JWT_Token(int user_Id);
    }
}
