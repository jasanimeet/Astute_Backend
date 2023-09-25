using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Quote_Master
    {
        [Key]
        public Int16 Quote_Id { get; set; }
        public string Quote { get; set; }
        public string Author { get; set; }
        public bool Status { get; set; }
        public Int16 Sr { get; set; }
    }
}
