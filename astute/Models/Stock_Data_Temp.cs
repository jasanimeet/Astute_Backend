using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Stock_Data_Temp
    {
        [Key]
        public int Stock_Data_Id { get; set; }
        [JsonProperty("SUPPLIER_NO")]
        public object? SUPPLIER_NO { get; set; }
        [JsonProperty("CERTIFICATE_NO")]
        public object? CERTIFICATE_NO { get; set; }
        [JsonProperty("LAB")]
        public object? LAB { get; set; }
        [JsonProperty("SHAPE")]
        public object? SHAPE { get; set; }
        [JsonProperty("CTS")]
        public object? CTS { get; set; }
        [JsonProperty("BASE_DISC")]
        public object? BASE_DISC { get; set; }
        [JsonProperty("BASE_RATE")]
        public object? BASE_RATE { get; set; }
        [JsonProperty("BASE_AMOUNT")]
        public object? BASE_AMOUNT { get; set; }
        [JsonProperty("COLOR")]
        public object? COLOR { get; set; }
        [JsonProperty("CLARITY")]
        public object? CLARITY { get; set; }
        [JsonProperty("CUT")]
        public object? CUT { get; set; }
        [JsonProperty("POLISH")]
        public object? POLISH { get; set; }
        [JsonProperty("SYMM")]
        public object? SYMM { get; set; }
        [JsonProperty("FLS_COLOR")]
        public object? FLS_COLOR { get; set; }
        [JsonProperty("FLS_INTENSITY")]
        public object? FLS_INTENSITY { get; set; }
        [JsonProperty("LENGTH")]
        public object? LENGTH { get; set; }
        [JsonProperty("WIDTH")]
        public object? WIDTH { get; set; }
        [JsonProperty("DEPTH")]
        public object? DEPTH { get; set; }
        [JsonProperty("MEASUREMENT")]
        public object? MEASUREMENT { get; set; }
        [JsonProperty("DEPTH_PER")]
        public object? DEPTH_PER { get; set; }
        [JsonProperty("TABLE_PER")]
        public object? TABLE_PER { get; set; }
        [JsonProperty("CULET")]
        public object? CULET { get; set; }
        [JsonProperty("SHADE")]
        public object? SHADE { get; set; }
        [JsonProperty("LUSTER")]
        public object? LUSTER { get; set; }
        [JsonProperty("MILKY")]
        public object? MILKY { get; set; }
        [JsonProperty("BGM")]
        public object? BGM { get; set; }
        [JsonProperty("LOCATION")]
        public object? LOCATION { get; set; }
        [JsonProperty("STATUS")]
        public object? STATUS { get; set; }
        [JsonProperty("TABLE_BLACK")]
        public object? TABLE_BLACK { get; set; }
        [JsonProperty("SIDE_BLACK")]
        public object? SIDE_BLACK { get; set; }
        [JsonProperty("TABLE_WHITE")]
        public object? TABLE_WHITE { get; set; }
        [JsonProperty("SIDE_WHITE")]
        public object? SIDE_WHITE { get; set; }
        [JsonProperty("TABLE_OPEN")]
        public object? TABLE_OPEN { get; set; }
        [JsonProperty("CROWN_OPEN")]
        public object? CROWN_OPEN { get; set; }
        [JsonProperty("PAVILION_OPEN")]
        public object? PAVILION_OPEN { get; set; }
        [JsonProperty("GIRDLE_OPEN")]
        public object? GIRDLE_OPEN { get; set; }
        [JsonProperty("GIRDLE_FROM")]
        public object? GIRDLE_FROM { get; set; }
        [JsonProperty("GIRDLE_TO")]
        public object? GIRDLE_TO { get; set; }
        [JsonProperty("GIRDLE_CONDITION")]
        public object? GIRDLE_CONDITION { get; set; }
        [JsonProperty("GIRDLE_TYPE")]
        public object? GIRDLE_TYPE { get; set; }
        [JsonProperty("LASER_INSCRIPTION")]
        public object? LASER_INSCRIPTION { get; set; }
        [JsonProperty("CERTIFICATE_DATE")]
        public object? CERTIFICATE_DATE { get; set; }
        [JsonProperty("CROWN_ANGLE")]
        public object? CROWN_ANGLE { get; set; }
        [JsonProperty("CROWN_HEIGHT")]
        public object? CROWN_HEIGHT { get; set; }
        [JsonProperty("PAVILION_ANGLE")]
        public object? PAVILION_ANGLE { get; set; }
        [JsonProperty("PAVILION_HEIGHT")]
        public object? PAVILION_HEIGHT { get; set; }
        [JsonProperty("GIRDLE_PER")]
        public object? GIRDLE_PER { get; set; }
        [JsonProperty("LR_HALF")]
        public object? LR_HALF { get; set; }
        [JsonProperty("STAR_LN")]
        public object? STAR_LN { get; set; }
        [JsonProperty("CERT_TYPE")]
        public object? CERT_TYPE { get; set; }
        [JsonProperty("FANCY_COLOR")]
        public object? FANCY_COLOR { get; set; }
        [JsonProperty("FANCY_INTENSITY")]
        public object? FANCY_INTENSITY { get; set; }
        [JsonProperty("FANCY_OVERTONE")]
        public object? FANCY_OVERTONE { get; set; }
        [JsonProperty("IMAGE_LINK")]
        public object? IMAGE_LINK { get; set; }
        [JsonProperty("Image2")]
        public object? Image2 { get; set; }
        [JsonProperty("VIDEO_LINK")]
        public object? VIDEO_LINK { get; set; }
        [JsonProperty("Video2")]
        public object? Video2 { get; set; }
        [JsonProperty("CERTIFICATE_LINK")]
        public object? CERTIFICATE_LINK { get; set; }
        [JsonProperty("DNA")]
        public object? DNA { get; set; }
        [JsonProperty("IMAGE_HEART_LINK")]
        public object? IMAGE_HEART_LINK { get; set; }
        [JsonProperty("IMAGE_ARROW_LINK")]
        public object? IMAGE_ARROW_LINK { get; set; }
        [JsonProperty("H_A_LINK")]
        public object? H_A_LINK { get; set; }
        [JsonProperty("CERTIFICATE_TYPE_LINK")]
        public object? CERTIFICATE_TYPE_LINK { get; set; }
        [JsonProperty("KEY_TO_SYMBOL")]
        public object? KEY_TO_SYMBOL { get; set; }
        [JsonProperty("LAB_COMMENTS")]
        public object? LAB_COMMENTS { get; set; }
        [JsonProperty("SUPPLIER_COMMENTS")]
        public object? SUPPLIER_COMMENTS { get; set; }
        [JsonProperty("ORIGIN")]
        public object? ORIGIN { get; set; }
        [JsonProperty("BOW_TIE")]
        public object? BOW_TIE { get; set; }
        [JsonProperty("EXTRA_FACET_TABLE")]
        public object? EXTRA_FACET_TABLE { get; set; }
        [JsonProperty("EXTRA_FACET_CROWN")]
        public object? EXTRA_FACET_CROWN { get; set; }
        [JsonProperty("EXTRA_FACET_PAVILION")]
        public object? EXTRA_FACET_PAVILION { get; set; }
        [JsonProperty("INTERNAL_GRAINING")]
        public object? INTERNAL_GRAINING { get; set; }
        [JsonProperty("H_A")]
        public object? H_A { get; set; }
        [JsonProperty("SUPPLIER_DISC")]
        public object? SUPPLIER_DISC { get; set; }
        [JsonProperty("SUPPLIER_AMOUNT")]
        public object? SUPPLIER_AMOUNT { get; set; }
        [JsonProperty("OFFER_DISC")]
        public object? OFFER_DISC { get; set; }
        [JsonProperty("OFFER_VALUE")]
        public object? OFFER_VALUE { get; set; }
        [JsonProperty("MAX_SLAB_BASE_DISC")]
        public object? MAX_SLAB_BASE_DISC { get; set; }
        [JsonProperty("MAX_SLAB_BASE_VALUE")]
        public object? MAX_SLAB_BASE_VALUE { get; set; }
        [JsonProperty("EYE_CLEAN")]
        public object? EYE_CLEAN { get; set; }
        [JsonProperty("Short_Code")]
        public object? Short_Code { get; set; }
        public bool? Is_Uploaded { get; set; }
    }
}
