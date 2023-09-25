using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Rapaport_Master
    {
        [Key]
        public int Rap_Id { get; set; }
        public string Insert_Date { get; set; }
    }
}
