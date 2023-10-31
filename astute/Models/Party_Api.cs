using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Party_Api
    {
        [Key]
        public int API_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? API_URL { get; set; }
        public string? API_User { get; set; }
        public string? API_Password { get; set; }
        public string? API_Method { get; set; }
        public string? API_Response { get; set; }
        public bool? API_Status { get; set; }
        public bool? Disc_Inverse { get; set; }
        public bool? Auto_Ref_No { get; set; }
        public string? RepeateveryType { get; set; }
        public string? Repeatevery { get; set; }
        public bool? Lab { get; set; }
        public bool? Overseas { get; set; }
        public string? Stock_Url { get; set; }
        public string? User_Id { get; set; }
        public string? User_Caption { get; set; }
        public string? Password_Caption { get; set; }
        public string? Action_Caption { get; set; }
        public string? Action_Value { get; set; }
        public string? Action_Caption_1 { get; set; }
        public string? Action_Value_1 { get; set; }
        public string? Action_Caption_2 { get; set; }
        public string? Action_Value_2 { get; set; }
        public string? Short_Code { get; set; }
        public string? Stock_Api_Method { get; set; }
        public string? Method_Type { get; set; }
    }
}
