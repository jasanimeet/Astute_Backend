using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Value_Config
    {
        [Key]
        public int ValueMap_ID { get; set; }
        public decimal? Length_From { get; set; }
        public decimal? Length_To { get; set; }
        public decimal? Width_From { get; set; }
        public decimal? Width_To { get; set; }
        public decimal? Depth_From { get; set; }
        public decimal? Depth_To { get; set; }
        public decimal? Depth_Per_From { get; set; }
        public decimal? Depth_Per_To { get; set; }
        public decimal? Table_Per_From { get; set; }
        public decimal? Table_Per_To { get; set; }
        public decimal? Crown_Angle_From { get; set; }
        public decimal? Crown_Angle_To { get; set; }
        public decimal? Crown_Height_From { get; set; }
        public decimal? Crown_Height_To { get; set; }
        public decimal? Pavilion_Angle_From { get; set; }
        public decimal? Pavilion_Angle_To { get; set; }
        public decimal? Pavilion_Height_From { get; set; }
        public decimal? Pavilion_Height_To { get; set; }
        public decimal? Girdle_Per_From { get; set; }
        public decimal? Girdle_Per_To { get; set; }
        public decimal? Lr_Half_From { get; set; }
        public decimal? Lr_Half_To { get; set; }
        public decimal? Star_Ln_From { get; set; }
        public decimal? Star_Ln_To { get; set; }
        public string? Shape_Group { get; set; }
        public string? Shape { get; set; }
        public string? Created_Date { get; set; }
        public string? Created_Time { get; set; }
        public string? Created_By { get; set; }
        public string? Updated_Date { get; set; }
        public string? Updated_Time { get; set; }
        public string? Updated_By { get; set; }

        [NotMapped]
        public int? User_Id { get; set; }

    }
}
