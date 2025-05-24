using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class GIA_Lab_Parameter
    {
        [Key]
        public int Id { get; set; }
        public string? Report_No { get; set; }
        public string? Report_Date { get; set; }
        public float? Length { get; set; }
        public float? Width { get; set; }
        public float? Depth { get; set; }
        public string? Color_Grade { get; set; }
        public string? Clarity_Grade { get; set; }
        public string? Cut_Grade { get; set; }
        public string? Polish_Grade { get; set; }
        public string? Symmetry_Grade { get; set; }
        public string? Fluorescence { get; set; }
        public string? Inscriptions { get; set; }
        public string? Key_To_Symbols { get; set; }
        public string? Report_Comments { get; set; }
        public float? Table_Pct { get; set; }
        public float? Depth_Pct { get; set; }
        public float? Crown_Angle { get; set; }
        public float? Crown_Height { get; set; }
        public float? Pavilion_Angle { get; set; }
        public float? Pavilion_Depth { get; set; }
        public float? Star_Length { get; set; }
        public float? Lower_Half { get; set; }
        public string? Shape_Code { get; set; }
        public string? Shape_Group { get; set; }
        public float? Carats { get; set; }
        public string? Clarity { get; set; }
        public string? Cut { get; set; }
        public string? Polish { get; set; }
        public string? Symmetry { get; set; }
        public string? Fluorescence_Intensity { get; set; }
        public string? Fluorescence_Color { get; set; }
        public string? Girdle_Condition { get; set; }
        public string? Girdle_Condition_Code { get; set; }
        public float? Girdle_Pct { get; set; }
        public string? Girdle_Size { get; set; }
        public string? Girdle_Size_Code { get; set; }
        public string? Culet_Code { get; set; }
        public string? Certificate_PDF { get; set; }
        public string? Plotting_Diagram { get; set; }
        public string? Proportions_Diagram { get; set; }
        public string? Digital_Card { get; set; }
    }
}