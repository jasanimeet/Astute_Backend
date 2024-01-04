using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Employee_Master
    {
        [Key]
        public int Employee_Id { get; set; }
        public string? Initial { get; set; }
        public string? First_Name { get; set; }
        public string? Middle_Name {get; set; }
        public string? Last_Name { get; set;}
        public string? Chinese_Name { get; set; }
        public string? Address_1 { get; set; }
        public string? Address_2 { get; set; }
        public string? Address_3 { get; set; }
        public int? City_Id { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Join_date { get; set; }
        public string? Employee_Type { get; set; }
        public string? Employee_Types { get; set; }
        public string? Birth_Date { get; set; }
        public string? Gender { get; set; }
        public string? Mobile_No { get; set; }
        public string? Personal_Email { get; set; }
        public string? Company_Email { get; set; }
        public string? Leave_Date { get; set; }
        public string? PSN_ID { get; set; }
        public string? Blood_Group { get; set; }
        public string? Contract_Start_Date { get; set; }
        public string? Contract_End_Date { get;set; }
        public Int16? Approve_Holidays { get; set; }
        public Int16? Order_No { get; set; }
        public Int16? Sort_No { get;set; }
        public string? User_Name { get; set; }
        public string? Password { get; set; }
        public bool? IsDelete { get; set; }
        public string? Employee_Code { get; set; }
        public bool? Status { get; set; }
        public string? Marital_Status { get; set; }
        public string? Mobile_Country_Code { get; set; }
        public string? Mobile_1_Country_Code { get; set; }
        public string? Probation_End_Date { get; set; }
        public string? Personal_Mobile_No { get; set; }
        public bool? Is_Admin { get; set; }
        public int? Designation_Id { get; set; }
        public string? Designation { get; set; }
        public string? User_Type { get; set; }
        [NotMapped]
        public IList<Employee_Document> Employee_Document_List { get; set; } = new List<Employee_Document>();
        [NotMapped]
        public IList<Employee_Salary> Employee_Salary_List { get; set; } = new List<Employee_Salary>();

        [NotMapped]
        public IList<Emergency_Contact_Detail> Emergency_Contact_Detail_List { get; set; } = new List<Emergency_Contact_Detail>();
    }
}
