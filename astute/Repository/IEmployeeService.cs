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
        Task<int> Get_Employee_Code();
        Task<IList<Employee_Master>> Get_Active_Employees(int employeeId, string userName, string personalEmail);
        Task Insert_Employee_Document_Trace(DataTable dataTable);
        Task Insert_Employee_Salary_Trace(DataTable dataTable);
        Task Insert_Emergency_Contact_Detail_Trace(DataTable dataTable);
        Task<IList<DropdownModel>> Get_Employee_For_Report(bool is_Exist, int rm_Id);
        Task<int> Employee_Master_Change_Status(int employee_Id, bool status);
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
        Task<Employee_Mail> Get_Employee_Email_Details(int user_Id);
        #endregion

        #region Emergency Contact Detail
        Task<int> Insert_Emergency_Contact_Detail(DataTable dataTable);
        #endregion

        Task<IList<DropdownModel>> Get_Buyer();
    }
}
