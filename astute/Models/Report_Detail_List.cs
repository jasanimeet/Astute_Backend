using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Report_Detail_List
    {
        public Report_Detail_List()
        {
            report_Filter_Parameter = new List<Report_Filter_Parameter>();
            report_Column_Parameter = new List<Report_Column_Parameter>();
        }
        public int Id { get; set; }

        public IList<Report_Filter_Parameter> report_Filter_Parameter { get; set; }
        public IList<Report_Column_Parameter> report_Column_Parameter { get; set; }


    }
    public class Report_Filter_Parameter
    {
        public int Id { get; set; }
        public int Col_Id { get; set; }
        public string Display_Name { get; set; }

    }

    public class Report_Column_Parameter
    {
        public int Id { get; set; }
        public int Rm_Id { get; set; }
        public string Display_Name { get; set; }
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
}
