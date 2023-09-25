using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Loader_Master
    {
        [Key]
        public int Loader_Id { get; set; }
        public string Loader_Img { get; set; }
        public bool Default_Loader { get; set; }
        [NotMapped]
        public bool IsEmployee_Loader { get; set; }
    }
}
