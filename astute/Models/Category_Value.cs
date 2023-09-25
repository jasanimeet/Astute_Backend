using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Category_Value
    {
        [Key]
        public int Cat_val_Id { get; set; }
        public string Cat_Name { get; set; }
        public string Group_Name { get; set; }
        public string Rapaport_Name { get; set; }
        public string Rapnet_name { get; set; }
        public string Synonyms { get; set; }
        public Int16 Order_No { get; set; }
        public Int16 Sort_No { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public string Preview_Icon { get; set; }
        public string Icon_Url { get; set; }
        public int? Cat_Id { get; set; }
        public string Display_Name { get; set; }
        public string Short_Name { get; set; }
        [NotMapped]
        public bool IsForceInsert { get; set; }
    }
}
