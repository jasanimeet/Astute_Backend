using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class Value_Config
    {
        [Key]
        public int ValueMap_ID { get; set; }
        public decimal Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Depth_Per { get; set; }
        public decimal? Table_Per { get; set; }
        public decimal? Crown_Angle { get; set; }
        public decimal? Crown_Height { get; set; }
        public decimal? Pavilion_Angle { get; set; }
        public decimal? Pavilion_Height { get; set; }
        public decimal? Girdle_Per { get; set; }
        public decimal? Lr_Half { get; set; }
        public decimal? Star_Ln { get; set; }
        public string? Shape_Group { get; set; }
        public string? Shape { get; set; }
        public string? Trans_Date { get; set; }
    }
}
