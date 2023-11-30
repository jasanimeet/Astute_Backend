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
        public string? Short_Code { get; set; }
        public bool? Is_Uploaded { get; set; }
    }
}
