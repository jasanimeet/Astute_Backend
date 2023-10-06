using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace astute.Repository
{
    public partial interface IEmployeeService
    {
        #region Employee Master
        Task<(string, int)> AddUpdateEmployee(Employee_Master employee_Master);
        Task<Employee_Master> Get_Employee_Details(int emoployee_Id);
        Task<int> UpdateEmployee(Employee_Master employee_Master);
        Task<(string, int)> DeleteEmployee(int employeeId);
        Task<IList<Employee_Master>> GetEmployees(int employeeId, string userName, string personalEmail);
        Task<Employee_Master> EmployeeLogin(UserModel userModel);
        Task<AuthenticateResponse> AuthenticateEmployee(UserModel userModel);
        #endregion

        #region Employee Document
        Task<int> InsertEmployeeDocument(DataTable dataTable);
        #endregion

        #region Employee Salary
        Task<int> InsertEmployeeSalary(DataTable dataTable);
        #endregion

        #region Employee Mail
        Task<IList<Employee_Mail>> GetEmployeeMail(int employeeId);
        Task<int> InsertEmployeeMail(Employee_Mail employee_Mail);
        Task<int> UpdateEmployeeMail(Employee_Mail employee_Mail);
        Task<int> DeleteEmployeeMail(int employeeId);
        #endregion

        #region Emergency Contact Detail
        Task<int> Insert_Emergency_Contact_Detail(DataTable dataTable);
        #endregion
    }
}
