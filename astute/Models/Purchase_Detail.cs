using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace astute.Models
{
    public partial class Purchase_Detail
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("Trans_Id")]
        public int? TransId { get; set; }

        [JsonProperty("Lab_Entry_Detail_Id")]
        public int? Lab_Entry_Detail_Id { get; set; }

        [JsonProperty("STOCK ID")]
        public string? StockId { get; set; }

        [JsonProperty("CERTIFICATE NO")]
        public string? CertificateNo { get; set; }

        [JsonProperty("Supplier_Id")]
        public int? SupplierId { get; set; }

        [JsonProperty("Supplier_Name")]
        public string? SupplierName { get; set; }

        [JsonProperty("Supplier_Short_Name")]
        public string? SupplierShortName { get; set; }

        [JsonProperty("SUPPLIER NO")]
        public string? SupplierNo { get; set; }

        [JsonProperty("STATUS")]
        public string? Status { get; set; }

        [JsonProperty("REMARKS")]
        public string? Remarks { get; set; }

        [JsonProperty("BUYER")]
        public string? Buyer { get; set; }

        [JsonProperty("BUYER CODE")]
        public int? BuyerCode { get; set; }

        [JsonProperty("LAB_Id")]
        public int? LabId { get; set; }

        [JsonProperty("Shape_Id")]
        public int? ShapeId { get; set; }

        [JsonProperty("BGM_Id")]
        public int? BgmId { get; set; }

        [JsonProperty("Color_Id")]
        public int? ColorId { get; set; }

        [JsonProperty("Clarity_Id")]
        public int? ClarityId { get; set; }

        [JsonProperty("CTS")]
        public string? Cts { get; set; }

        [JsonProperty("RAP RATE")]
        public string? RapRate { get; set; }

        [JsonProperty("RAP AMOUNT")]
        public string? RapAmount { get; set; }

        [JsonProperty("BASE DISC")]
        public string? BaseDisc { get; set; }

        [JsonProperty("BASE AMOUNT")]
        public string? BaseAmount { get; set; }

        [JsonProperty("COST DISC")]
        public string? CostDisc { get; set; }

        [JsonProperty("COST AMOUNT")]
        public string? CostAmount { get; set; }

        [JsonProperty("OFFER DISC")]
        public string? OfferDisc { get; set; }

        [JsonProperty("OFFER AMOUNT")]
        public string? OfferAmount { get; set; }

        [JsonProperty("ACTUAL COST DISC")]
        public string? ActualCostDisc { get; set; }

        [JsonProperty("ACTUAL COST AMOUNT")]
        public string? ActualCostAmount { get; set; }

        [JsonProperty("Cut_Id")]
        public int? CutId { get; set; }

        [JsonProperty("Polish_Id")]
        public int? PolishId { get; set; }

        [JsonProperty("Symm_Id")]
        public int? SymmId { get; set; }

        [JsonProperty("FLS_INTENSITY_Id")]
        public int? FlsintensityId { get; set; }

        [JsonProperty("LENGTH")]
        public string? Length { get; set; }

        [JsonProperty("WIDTH")]
        public string? Width { get; set; }

        [JsonProperty("DEPTH")]
        public string? Depth { get; set; }

        [JsonProperty("DEPTH PER")]
        public string? DepthPer { get; set; }

        [JsonProperty("TABLE PER")]
        public string? TablePer { get; set; }

        [JsonProperty("KEY TO SYMBOL")]
        public string? KeyToSymbol { get; set; }

        [JsonProperty("GIA COMMENTS")]
        public string? GiaComments { get; set; }

        [JsonProperty("GIRDLE PER")]
        public string? GirdlePer { get; set; }

        [JsonProperty("CROWN ANGLE")]
        public string? CrownAngle { get; set; }

        [JsonProperty("CROWN HEIGHT")]
        public string? CrownHeight { get; set; }

        [JsonProperty("PAVILION ANGLE")]
        public string? PavilionAngle { get; set; }

        [JsonProperty("PAVILION HEIGHT")]
        public string? PavilionHeight { get; set; }

        [JsonProperty("Table_Black_Id")]
        public int? TableBlackId { get; set; }

        [JsonProperty("Crown_Black_Id")]
        public int? CrownBlackId { get; set; }

        [JsonProperty("Table_White_Id")]
        public int? TableWhiteId { get; set; }

        [JsonProperty("Crown_White_Id")]
        public int? CrownWhiteId { get; set; }

        [JsonProperty("Culet_Id")]
        public int? CuletId { get; set; }

        [JsonProperty("Table_Open_Id")]
        public int? TableOpenId { get; set; }

        [JsonProperty("Crown_Open_Id")]
        public int? CrownOpenId { get; set; }

        [JsonProperty("Pav_Open_Id")]
        public int? PavOpenId { get; set; }

        [JsonProperty("Girdle_Open_Id")]
        public int? GirdleOpenId { get; set; }

        [JsonProperty("CERTIFICATE DATE")]
        public string? CertificateDate { get; set; }

        [JsonProperty("Cert_Type_Id")]
        public int? CertTypeId { get; set; }

        [JsonProperty("LR HALF")]
        public int? LrHalf { get; set; }

        [JsonProperty("STAR LN")]
        public int? StarLn { get; set; }

        [JsonProperty("Fancy_Color_Id")]
        public int? FancyColorId { get; set; }

        [JsonProperty("Fancy_intensity_Id")]
        public int? FancyintensityId { get; set; }

        [JsonProperty("Fancy_Overtone_Id")]
        public int? FancyOvertoneId { get; set; }

        [JsonProperty("Rough_Origin_Id")]
        public int? RoughOriginId { get; set; }

        [JsonProperty("IMAGE LINK")]
        public string? ImageLink { get; set; }

        [JsonProperty("VIDEO LINK")]
        public string? VideoLink { get; set; }

        [JsonProperty("CERTIFICATE LINK")]
        public string? CertificateLink { get; set; }

        [JsonProperty("DNA")]
        public string? Dna { get; set; }

        [JsonProperty("Cert_Type_Link")]
        public string? CertTypeLink { get; set; }

        [JsonProperty("LAB")]
        public string? Lab { get; set; }

        [JsonProperty("SHAPE")]
        public string? Shape { get; set; }

        [JsonProperty("BGM")]
        public string? Bgm { get; set; }

        [JsonProperty("COLOR")]
        public string? Color { get; set; }

        [JsonProperty("CLARITY")]
        public string? Clarity { get; set; }

        [JsonProperty("CUT")]
        public string? Cut { get; set; }

        [JsonProperty("POLISH")]
        public string? Polish { get; set; }

        [JsonProperty("SYMM")]
        public string? Symm { get; set; }

        [JsonProperty("FLS INTENSITY")]
        public string? Flsintensity { get; set; }

        [JsonProperty("TABLE WHITE")]
        public string? TableWhite { get; set; }

        [JsonProperty("SIDE WHITE")]
        public string? SideWhite { get; set; }

        [JsonProperty("TABLE BLACK")]
        public string? TableBlack { get; set; }

        [JsonProperty("SIDE BLACK")]
        public string? SideBlack { get; set; }

        [JsonProperty("CULET")]
        public string? Culet { get; set; }

        [JsonProperty("TABLE OPEN")]
        public string? TableOpen { get; set; }

        [JsonProperty("CROWN OPEN")]
        public string? CrownOpen { get; set; }

        [JsonProperty("PAVILION OPEN")]
        public string? PavilionOpen { get; set; }

        [JsonProperty("GIRDLE OPEN")]
        public string? GirdleOpen { get; set; }

        [JsonProperty("GIA TYPE")]
        public string? GiaType { get; set; }

        [JsonProperty("FANCY COLOR")]
        public string? FancyColor { get; set; }

        [JsonProperty("FANCY INTENSITY")]
        public string? Fancyintensity { get; set; }

        [JsonProperty("FANCY OVERTONE")]
        public string? FancyOvertone { get; set; }

        [JsonProperty("Rough_Origin")]
        public string? RoughOrigin { get; set; }

        [JsonProperty("LASER INSCRIPTION")]
        public string? LaserInscription { get; set; }

        [JsonProperty("Laser_Insc_Id")]
        public int? LaserInscId { get; set; }

        [JsonProperty("Girdle_Condition_Id")]
        public int? GirdleConditionId { get; set; }

        [JsonProperty("GIRDLE CONDITION")]
        public string? GirdleCondition { get; set; }

        [JsonProperty("Stone_Status")]
        public string? Stone_Status { get; set; }
        
        [JsonProperty("Shipment_Type")]
        public string? Shipment_Type { get; set; }
        
        [JsonProperty("Purchase_Doc_No")]
        public string? Purchase_Doc_No { get; set; }
        
        [JsonProperty("Expected_Delivery_Date")]
        public string? Expected_Delivery_Date { get; set; }
        
        [JsonProperty("Web_Disc")]
        public string? Web_Disc { get; set; }
        
        [JsonProperty("Web_Amount")]
        public string? Web_Amount { get; set; }
        
        [JsonProperty("Sunrise_Stock_Id")]
        public string? Sunrise_Stock_Id { get; set; }
        
        [JsonProperty("Sunrise_Status")]
        public string? Sunrise_Status { get; set; }
        
        [JsonProperty("RFID_No")]
        public string? RFID_No { get; set; }
        
        [JsonProperty("Supp_Verified_Disc")]
        public string? Supp_Verified_Disc { get; set; }
        
        [JsonProperty("Supp_Verified_Amount")]
        public string? Supp_Verified_Amount { get; set; }
        
        [JsonProperty("Sunrise_Offer_Disc")]
        public string? Sunrise_Offer_Disc { get; set; }
        
        [JsonProperty("Sunrise_Offer_Amt")]
        public string? Sunrise_Offer_Amt { get; set; }
        
        [JsonPropertyName("COMPANY")]
        public string? Company { get; set; }
    }
}
