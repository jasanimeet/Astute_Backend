using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    [Keyless]
    public class QC_Master
    {
        public int Trans_Id { get; set; }
        public string Criteria_Name { get; set; }
        public bool? Status { get; set; }
        public string? QC_Type { get; set; }
        public string? Created_By { get; set; }
        public string? Created_Date { get; set; }
        public string? Created_Time { get; set; }
        public string? Updated_By { get; set; }
        public string? Updated_Date { get; set; }
        public string? Updated_Time { get; set; }
        public IList<QC_Detail> QC_Detail_List { get; set; }
    }
    public class QC_Detail
    {
        public int Id { get; set; }
        public int Trans_Id { get; set; }
        public string QC_Name { get; set; }
        public bool Status { get; set; }
    }
    public class QC_Master_Model
    {
        public int Trans_Id { get; set; }
        public string Criteria_Name { get; set; }
        public bool? Status { get; set; }
        public string? QC_Type { get; set; }
        public string Query_Flag { get; set; }
    }
}
