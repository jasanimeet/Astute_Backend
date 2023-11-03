using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Party_Assist
    {
        [Key]
        public int Assist_Id { get; set; }
        public int? Party_Id { get; set; }
        public string? Party_Name { get; set; }
        public int? Diamond_Type { get; set; }
        public string? Diamond_Type_Value { get; set; }
        public int? Assist_1 { get; set; }
        public string? Assist_Value_1 { get; set; }
        public decimal? Per_1 { get; set; }
        public int? Assist_2 { get; set; }
        public string? Assist_Value_2 { get; set; }
        public decimal? Per_2 { get; set; }
        public string? Viewing_Rights_To { get; set; }
        public string? Viewing_Rights_To_Values { get; set; }
        public string? Date { get; set; }
        [NotMapped]
        public string? QueryFlag { get; set; }
    }
}
