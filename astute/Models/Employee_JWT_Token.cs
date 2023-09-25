using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Employee_JWT_Token
    {
        [Key]
        public int Token_Id { get; set; }
        public int? Employee_Id { get; set; }
        public string? IP_Address { get; set; }        
        public string? Token { get; set; }
    }
}
