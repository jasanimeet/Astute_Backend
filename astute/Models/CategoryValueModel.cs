using System;

namespace astute.Models
{
    public class CategoryValueModel
    {
        public int Cat_val_Id { get; set; }
        public string Cat_Name { get; set; }
        public string Group_Name { get; set; }
        public string Rapaport_Name { get; set; }
        public string Rapnet_name { get; set; }
        public string Synonyms { get; set; }
        public Int16 Order_No { get; set; }
        public Int16 Sort_No { get; set; }
        public bool Status { get; set; }
        public string Icon_Url { get; set; }
        public int? Cat_Id { get; set; }
        public string Display_Name { get; set; }
        public string Short_Name { get; set; }
    }
}
