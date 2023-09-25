using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Currency_Master
    {
        [Key]
        public int Currency_Id { get; set; }
        public string Currency { get; set; }
        public string Currency_Name { get; set; }
        public string Symbol { get; set; }
        public Int16 Order_No { get; set; }
        public Int16 Sort_No { get; set; }
        public bool status { get; set; }
        [NotMapped]
        public bool IsForceInsert { get; set; }
    }
}
