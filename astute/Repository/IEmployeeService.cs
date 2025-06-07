using astute.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static astute.Models.Employee_Master;

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
        Task<Employee_Fortune_Master> GetEmployeeFortuneId(int employeeId);
        Task<Employee_Fortune_Order_Master> GetEmployeeFortuneIdByOrderNo(string order_No);
        Task<Employee_Master> EmployeeLogin(UserModel userModel);
        Task<AuthenticateResponse> AuthenticateEmployee(UserModel userModel);
        Task<int> Update_FCMToken(UserModel userModel, int Id);
        Task<int> Get_Employee_Code();
        Task<IList<Employee_Master>> Get_Active_Employees(int employeeId, string userName, string personalEmail);
        Task<IList<Employee_Master>> Get_Active_Secretary_Employees(int user_Id);
        Task Insert_Employee_Document_Trace(DataTable dataTable);
        Task Insert_Employee_Salary_Trace(DataTable dataTable);
        Task Insert_Emergency_Contact_Detail_Trace(DataTable dataTable);
        Task<IList<DropdownModel>> Get_Employee_For_Report(bool is_Exist, int rm_Id, int user_Id, string user_Type);
        Task<int> Employee_Master_Change_Status(int employee_Id, bool status);
        Task<(string, int)> Change_Password(Change_Password_Model change_Password_Model, int? user_Id);
        Task<List<DropdownModel>> Employee_Master_Name_Select(int employee_Id);
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
        Task<Employee_Mail> Get_Employee_Email_Or_Default_Email(int user_Id);
        //Task<List<Dictionary<string, object>>> Get_Employee_Email_Details(int user_Id);
        #endregion

        #region Emergency Contact Detail
        Task<int> Insert_Emergency_Contact_Detail(DataTable dataTable);
        #endregion

        #region Buyer List
        Task<IList<DropdownModel>> Get_Buyer();
        Task<IList<DropdownModel>> Get_Buyer_List();
        #endregion

        #region Secretary List
        Task<IList<DropdownModel>> Get_Secretary();
        #endregion

        #region Employee Secretary
        Task<int> Insert_Update_Delete_Employee_Secretary(DataTable dataTable);
        #endregion
        Task<IList<DropdownModel>> Get_Employee_Master_By_User_Type(string user_Type);
    }
}
