using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    [Keyless]
    public partial class Quote_Mas_Model
    {
        public string Quote { get; set; }
        public string Author { get; set; }
    }
}
