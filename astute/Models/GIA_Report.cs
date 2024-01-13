using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace astute.Models
{
    public partial class GIA_Report
    {
        [JsonProperty("data")]
        public GIA_Data Data { get; set; }
    }

    public partial class GIA_Data
    {
        [JsonProperty("getReport")]
        public GetReport GetReport { get; set; }
    }

    public partial class GetReport
    {
        [JsonProperty("report_number")]
        public string ReportNumber { get; set; }

        [JsonProperty("report_date")]
        public string ReportDate { get; set; }

        [JsonProperty("results")]
        public Results Results { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("quota")]
        public Quota Quota { get; set; }
    }

    public partial class Links
    {
        [JsonProperty("pdf")]
        public Uri Pdf { get; set; }

        [JsonProperty("proportions_diagram")]
        public Uri ProportionsDiagram { get; set; }

        [JsonProperty("plotting_diagram")]
        public Uri PlottingDiagram { get; set; }

        [JsonProperty("digital_card")]
        public object DigitalCard { get; set; }
    }

    public partial class Quota
    {
        [JsonProperty("remaining")]
        public long Remaining { get; set; }
    }

    public partial class Results
    {
        [JsonProperty("__typename")]
        public string Typename { get; set; }

        [JsonProperty("measurements")]
        public string Measurements { get; set; }

        [JsonProperty("carat_weight")]
        public string CaratWeight { get; set; }

        [JsonProperty("color_grade")]
        public string ColorGrade { get; set; }

        [JsonProperty("color_origin")]
        public object ColorOrigin { get; set; }

        [JsonProperty("color_distribution")]
        public object ColorDistribution { get; set; }

        [JsonProperty("clarity_grade")]
        public string ClarityGrade { get; set; }

        [JsonProperty("cut_grade")]
        public string CutGrade { get; set; }

        [JsonProperty("polish")]
        public string Polish { get; set; }

        [JsonProperty("symmetry")]
        public string Symmetry { get; set; }

        [JsonProperty("fluorescence")]
        public string Fluorescence { get; set; }

        [JsonProperty("clarity_characteristics")]
        public object ClarityCharacteristics { get; set; }

        [JsonProperty("key_to_symbols")]
        public List<KeyToSymbols> KeyToSymbols { get; set; }

        [JsonProperty("inscriptions")]
        public string Inscriptions { get; set; }

        [JsonProperty("report_comments")]
        public string ReportComments { get; set; }

        [JsonProperty("proportions")]
        public Proportions Proportions { get; set; }

        [JsonProperty("data")]
        public ResultsData Data { get; set; }
    }

    public class KeyToSymbols
    {
        public string characteristic { get; set; }
    }

    public partial class ResultsData
    {
        [JsonProperty("shape")]
        public Shape Shape { get; set; }

        [JsonProperty("weight")]
        public Weight Weight { get; set; }

        [JsonProperty("color")]
        public Color Color { get; set; }

        [JsonProperty("clarity")]
        public string Clarity { get; set; }

        [JsonProperty("cut")]
        public string Cut { get; set; }

        [JsonProperty("polish")]
        public string Polish { get; set; }

        [JsonProperty("symmetry")]
        public string Symmetry { get; set; }

        [JsonProperty("fluorescence")]
        public Fluorescence Fluorescence { get; set; }

        [JsonProperty("girdle")]
        public Girdle Girdle { get; set; }

        [JsonProperty("culet")]
        public Culet Culet { get; set; }
    }

    public partial class Color
    {
        [JsonProperty("color_grade_code")]
        public string ColorGradeCode { get; set; }

        [JsonProperty("color_modifier")]
        public object ColorModifier { get; set; }
    }

    public partial class Culet
    {
        [JsonProperty("culet_code")]
        public string CuletCode { get; set; }
    }

    public partial class Fluorescence
    {
        [JsonProperty("fluorescence_intensity")]
        public string FluorescenceIntensity { get; set; }

        [JsonProperty("fluorescence_color")]
        public object FluorescenceColor { get; set; }
    }

    public partial class Girdle
    {
        [JsonProperty("girdle_condition")]
        public string GirdleCondition { get; set; }

        [JsonProperty("girdle_condition_code")]
        public string GirdleConditionCode { get; set; }

        [JsonProperty("girdle_pct")]
        public string GirdlePct { get; set; }

        [JsonProperty("girdle_size")]
        public string GirdleSize { get; set; }

        [JsonProperty("girdle_size_code")]
        public string GirdleSizeCode { get; set; }
    }

    public partial class Shape
    {
        [JsonProperty("shape_category")]
        public string ShapeCategory { get; set; }

        [JsonProperty("shape_code")]
        public string ShapeCode { get; set; }

        [JsonProperty("shape_group")]
        public string ShapeGroup { get; set; }

        [JsonProperty("shape_group_code")]
        public string ShapeGroupCode { get; set; }
    }

    public partial class Weight
    {
        [JsonProperty("weight")]
        public string WeightWeight { get; set; }

        [JsonProperty("weight_unit")]
        public string WeightUnit { get; set; }
    }

    public partial class Proportions
    {
        [JsonProperty("depth_pct")]
        public string DepthPct { get; set; }

        [JsonProperty("table_pct")]
        public string TablePct { get; set; }

        [JsonProperty("crown_angle")]
        public string CrownAngle { get; set; }

        [JsonProperty("crown_height")]
        public string CrownHeight { get; set; }

        [JsonProperty("pavilion_angle")]
        public string PavilionAngle { get; set; }

        [JsonProperty("pavilion_depth")]
        public string PavilionDepth { get; set; }

        [JsonProperty("star_length")]
        public string StarLength { get; set; }

        [JsonProperty("lower_half")]
        public string LowerHalf { get; set; }

        [JsonProperty("girdle")]
        public string Girdle { get; set; }

        [JsonProperty("culet")]
        public string Culet { get; set; }
    }
}
