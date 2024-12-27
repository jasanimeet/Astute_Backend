using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Employee_Download_Share_Rights_Model
    {
        [Key]
        public int Employee_Id { get; set; }
        public IList<Employee_Download_Share_Rights_Post_Model> Employee_Download_Share_Rights_Post_Model { get; set; }
    }
}
