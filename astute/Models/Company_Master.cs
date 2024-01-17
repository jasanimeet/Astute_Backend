using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Company_Master
    {
        [Key]
        public int Company_Id { get; set; }
        public string? Company_Name { get; set; }
        public string? Address_1 { get; set; }
        public string? Address_2 { get; set; }
        public string? Address_3 { get; set; }
        public int? City_Id { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Country_Code { get; set; }
        public string? Phone_No { get; set; }
        public string? Fax_No { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public Int16? Order_No { get; set; }
        public Int16? Sort_No { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public IList<Company_Document> Company_Document_List { get; set; } = new List<Company_Document>();
        [NotMapped]
        public IList<Company_Media> Company_Media_List { get; set; } = new List<Company_Media>();
        [NotMapped]
        public IList<Company_Bank> Company_Bank_List { get; set; } = new List<Company_Bank>();
    }
}
