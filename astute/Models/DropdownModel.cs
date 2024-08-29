using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class DropdownModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public partial class PartyMasterDrop
    {
        [Key]
        public string Party_Code { get; set; }
        public string Party_Name { get; set; }
    }
}
