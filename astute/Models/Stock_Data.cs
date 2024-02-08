using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace astute.Models
{
    public partial class Stock_Data
    {
        [Key]
        public int Stock_Data_Id { get; set; }
        public string? SUPPLIER_NO { get; set; }
        public string? CERTIFICATE_NO { get; set; }
        public string? LAB { get; set; }
        public string? SHAPE { get; set; }
        public string? CTS { get; set; }
        public string? BASE_DISC { get; set; }
        public string? BASE_RATE { get; set; }
        public string? BASE_AMOUNT { get; set; }
        public string? COLOR { get; set; }
        public string? CLARITY { get; set; }
        public string? CUT { get; set; }
        public string? POLISH { get; set; }
        public string? SYMM { get; set; }
        public string? FLS_COLOR { get; set; }
        public string? FLS_INTENSITY { get; set; }        
        public string? LENGTH { get; set; }
        public string? WIDTH { get; set; }
        public string? DEPTH { get; set; }
        public string? MEASUREMENT { get; set; }
        public string? DEPTH_PER { get; set; }
        public string? TABLE_PER { get; set; }
        public string? CULET { get; set; }
        public string? SHADE { get; set; }
        public string? LUSTER { get; set; }
        public string? MILKY { get; set; }
        public string? BGM { get; set; }
        public string? LOCATION { get; set; }
        public string? STATUS { get; set; }
        public string? TABLE_BLACK { get; set; }
        public string? SIDE_BLACK { get; set; }
        public string? TABLE_WHITE { get; set; }
        public string? SIDE_WHITE { get; set; }
        public string? TABLE_OPEN { get; set; }
        public string? CROWN_OPEN { get; set; }
        public string? PAVILION_OPEN { get; set; }
        public string? GIRDLE_OPEN { get; set; }
        public string? GIRDLE_FROM { get; set; }
        public string? GIRDLE_TO { get; set; }
        public string? GIRDLE_CONDITION { get; set; }
        public string? GIRDLE_TYPE { get; set; }
        public string? LASER_INSCRIPTION { get; set; }
        public string? CERTIFICATE_DATE { get; set; }
        public string? CROWN_ANGLE { get; set; }
        public string? CROWN_HEIGHT { get; set; }
        public string? PAVILION_ANGLE { get; set; }
        public string? PAVILION_HEIGHT { get; set; }
        public string? GIRDLE_PER { get; set; }
        public string? LR_HALF { get; set; }
        public string? STAR_LN { get; set; }
        public string? CERT_TYPE { get; set; }
        public string? FANCY_COLOR { get; set; }
        public string? FANCY_INTENSITY { get; set; }
        public string? FANCY_OVERTONE { get; set; }
        public string? IMAGE_LINK { get; set; }
        public string? Image2 { get; set; }
        public string? VIDEO_LINK { get; set; }
        public string? Video2 { get; set; }
        public string? CERTIFICATE_LINK { get; set; }
        public string? DNA { get; set; }
        public string? IMAGE_HEART_LINK { get; set; }
        public string? IMAGE_ARROW_LINK { get; set; }
        public string? H_A_LINK { get; set; }
        public string? CERTIFICATE_TYPE_LINK { get; set; }
        public string? KEY_TO_SYMBOL { get; set; }
        public string? LAB_COMMENTS { get; set; }
        public string? SUPPLIER_COMMENTS { get; set; }
        public string? ORIGIN { get; set; }
        public string? BOW_TIE { get; set; }
        public string? EXTRA_FACET_TABLE { get; set; }
        public string? EXTRA_FACET_CROWN { get; set; }
        public string? EXTRA_FACET_PAVILION { get; set; }
        public string? INTERNAL_GRAINING { get; set; }
        public string? H_A { get; set; }
        public string? SUPPLIER_DISC { get; set; }
        public string? SUPPLIER_AMOUNT { get; set; }
        public string? OFFER_DISC { get; set; }
        public string? OFFER_VALUE { get; set; }
        public string? MAX_SLAB_BASE_DISC { get; set; }
        public string? MAX_SLAB_BASE_VALUE { get; set; }
        public string? EYE_CLEAN { get; set; }
        public string? GOOD_TYPE { get; set; }
        public string? Short_Code { get; set; }
        public bool? Is_Uploaded { get; set; }
    }
    public partial class Stock_Data_Schedular
    {
        [Key]
        public int Stock_Data_Id { get; set; }
        public object? SUPPLIER_NO { get; set; }
        public object? CERTIFICATE_NO { get; set; }
        public object? LAB { get; set; }
        public object? SHAPE { get; set; }
        public object? Short_Code { get; set; }
        public object? CTS { get; set; }
        public object? BASE_DISC { get; set; }
        public object? BASE_RATE { get; set; }
        public object? BASE_AMOUNT { get; set; }
        public object? COLOR { get; set; }
        public object? CLARITY { get; set; }
        public object? CUT { get; set; }
        public object? POLISH { get; set; }
        public object? SYMM { get; set; }
        public object? FLS_COLOR { get; set; }
        public object? FLS_INTENSITY { get; set; }
        public object? LENGTH { get; set; }
        public object? WIDTH { get; set; }
        public object? DEPTH { get; set; }
        public object? MEASUREMENT { get; set; }
        public object? DEPTH_PER { get; set; }
        public object? TABLE_PER { get; set; }
        public object? CULET { get; set; }
        public object? SHADE { get; set; }
        public object? LUSTER { get; set; }
        public object? MILKY { get; set; }
        public object? BGM { get; set; }
        public object? LOCATION { get; set; }
        public object? STATUS { get; set; }
        public object? TABLE_BLACK { get; set; }
        public object? SIDE_BLACK { get; set; }
        public object? TABLE_WHITE { get; set; }
        public object? SIDE_WHITE { get; set; }
        public object? TABLE_OPEN { get; set; }
        public object? CROWN_OPEN { get; set; }
        public object? PAVILION_OPEN { get; set; }
        public object? GIRDLE_OPEN { get; set; }
        public object? GIRDLE_FROM { get; set; }
        public object? GIRDLE_TO { get; set; }
        public object? GIRDLE_CONDITION { get; set; }
        public object? GIRDLE_TYPE { get; set; }
        public object? LASER_INSCRIPTION { get; set; }
        public object? CERTIFICATE_DATE { get; set; }
        public object? CROWN_ANGLE { get; set; }
        public object? CROWN_HEIGHT { get; set; }
        public object? PAVILION_ANGLE { get; set; }
        public object? PAVILION_HEIGHT { get; set; }
        public object? GIRDLE_PER { get; set; }
        public object? LR_HALF { get; set; }
        public object? STAR_LN { get; set; }
        public object? CERT_TYPE { get; set; }
        public object? FANCY_COLOR { get; set; }
        public object? FANCY_INTENSITY { get; set; }
        public object? FANCY_OVERTONE { get; set; }
        public object? IMAGE_LINK { get; set; }
        public object? Image2 { get; set; }
        public object? VIDEO_LINK { get; set; }
        public object? Video2 { get; set; }
        public object? CERTIFICATE_LINK { get; set; }
        public object? DNA { get; set; }
        public object? IMAGE_HEART_LINK { get; set; }
        public object? IMAGE_ARROW_LINK { get; set; }
        public object? H_A_LINK { get; set; }
        public object? CERTIFICATE_TYPE_LINK { get; set; }
        public object? KEY_TO_SYMBOL { get; set; }
        public object? LAB_COMMENTS { get; set; }
        public object? SUPPLIER_COMMENTS { get; set; }
        public object? ORIGIN { get; set; }
        public object? BOW_TIE { get; set; }
        public object? EXTRA_FACET_TABLE { get; set; }
        public object? EXTRA_FACET_CROWN { get; set; }
        public object? EXTRA_FACET_PAVILION { get; set; }
        public object? INTERNAL_GRAINING { get; set; }
        public object? H_A { get; set; }
        public object? SUPPLIER_DISC { get; set; }
        public object? SUPPLIER_AMOUNT { get; set; }
        public object? OFFER_DISC { get; set; }
        public object? OFFER_VALUE { get; set; }
        public object? MAX_SLAB_BASE_DISC { get; set; }
        public object? MAX_SLAB_BASE_VALUE { get; set; }
        public object? EYE_CLEAN { get; set; }
        public object? GOOD_TYPE { get; set; }
        public bool? Is_Uploaded { get; set; }
    }
    public partial class Supplier_Stock_Excel
    {
        [JsonProperty("SUPPLIER_NO")]
        public string SUPPLIER_NO { get; set; }

        [JsonProperty("CERTIFICATE_NO")]
        public string CERTIFICATE_NO { get; set; }

        [JsonProperty("LAB")]
        public string Lab { get; set; }

        [JsonProperty("SHAPE")]
        public string Shape { get; set; }

        [JsonProperty("CTS")]
        public string Cts { get; set; }

        [JsonProperty("Supplier Name")]
        public string SupplierName { get; set; }

        [JsonProperty("Stock Status")]
        public string StockStatus { get; set; }

        [JsonProperty("Error Message")]
        public string ErrorMessage { get; set; }

        [JsonProperty("COLOR")]
        public string Color { get; set; }

        [JsonProperty("CLARITY")]
        public string Clarity { get; set; }

        [JsonProperty("CUT")]
        public string Cut { get; set; }

        [JsonProperty("POLISH")]
        public string Polish { get; set; }

        [JsonProperty("SYMM")]
        public string Symm { get; set; }

        [JsonProperty("FLS_INTENSITY")]
        public string FlsIntensity { get; set; }

        [JsonProperty("LENGTH")]
        public string Length { get; set; }

        [JsonProperty("WIDTH")]
        public string Width { get; set; }

        [JsonProperty("DEPTH")]
        public string Depth { get; set; }

        [JsonProperty("DEPTH_PER")]
        public string DepthPer { get; set; }

        [JsonProperty("TABLE_PER")]
        public string TablePer { get; set; }

        [JsonProperty("CULET")]
        public string Culet { get; set; }

        [JsonProperty("SHADE")]
        public string Shade { get; set; }

        [JsonProperty("MILKY")]
        public string Milky { get; set; }

        [JsonProperty("LOCATION")]
        public string Location { get; set; }

        [JsonProperty("Upload Type")]
        public string UploadType { get; set; }

        [JsonProperty("TABLE_BLACK")]
        public string TableBlack { get; set; }

        [JsonProperty("SIDE_BLACK")]
        public string SideBlack { get; set; }

        [JsonProperty("TABLE_WHITE")]
        public string TableWhite { get; set; }

        [JsonProperty("SIDE_WHITE")]
        public string SideWhite { get; set; }

        [JsonProperty("CROWN_ANGLE")]
        public string CrownAngle { get; set; }

        [JsonProperty("PAVILION_ANGLE")]
        public string PavilionAngle { get; set; }

        [JsonProperty("GIRDLE_PER")]
        public string GirdlePer { get; set; }

        [JsonProperty("KEY_TO_SYMBOL")]
        public string KeyToSymbol { get; set; }

        [JsonProperty("Stock_Type")]
        public string StockType { get; set; }
    }

}
