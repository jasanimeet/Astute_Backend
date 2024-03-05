using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    [Keyless]
    public class Report_Detail
    {
        public int Id { get; set; }
        public int Rm_Id { get; set; }
        public string Display_Name { get; set; }
        public string Column_Type { get; set; }
        public int Col_Id { get; set; }
        public string Order_By { get; set; }
        public int Short_No { get; set; }
        public string Display_Type { get; set; }
        public int Width { get; set; }
        public string Column_Format { get; set; }
        public string Alignment { get; set; }
        public string Fore_Colour { get; set; }
        public string Back_Colour { get; set; }
        public bool IsBold { get; set; }

    }

    public class Report_Filter_Parameter
    {
        public int Col_Id { get; set; }
        public string Column_Name { get; set; }
        public string Category_Value { get; set; }
    }

    public class Report_Filter
    {
        public int id { get; set; }
        public IList<Report_Filter_Parameter> Report_Filter_Parameter { get; set; } = new List<Report_Filter_Parameter>();
        public int? iPgNo { get; set; }
        public int? iPgSize { get; set; }
        public IList<Report_Sorting> iSort { get; set; } = new List<Report_Sorting>();
    }
    public class Report_Lab_Filter
    {
        public int id { get; set; }
        public IList<IList<Report_Multiple_Filter_Parameter>> Report_Filter_Parameter_List { get; set; } = new List<IList<Report_Multiple_Filter_Parameter>>();
        //public IList<Report_Filter_Parameter_List> Report_Filter_Parameter_List { get; set; } = new List<Report_Filter_Parameter_List>();
        public int? iPgNo { get; set; }
        public int? iPgSize { get; set; }
        public IList<Report_Sorting> iSort { get; set; } = new List<Report_Sorting>();
    }
    public class Report_Filter_Parameter_List
    {
        public IList<Report_Multiple_Filter_Parameter> Report_Filter_Parameter { get; set; } = new List<Report_Multiple_Filter_Parameter>();

    }
    public class Report_Multiple_Filter_Parameter
    {
        public int Col_Id { get; set; }
        public string Column_Name { get; set; }
        public string Category_Value { get; set; }
        public string Category_Name { get; set; }
        public string? KEY_TO_SYMBOL_TRUE { get; set; }
        public string? KEY_TO_SYMBOL_FALSE { get; set; }
        public string? LAB_COMMENTS_TRUE { get; set; }
        public string? LAB_COMMENTS_FALSE { get; set; }
    }
    public class Report_Sorting
    {
        public string sort { get; set; }
        public string col_name { get; set; }
    }

    public class Report_Filter_Display
    { 
      public string Filter { get; set; }
    }
}
