using Newtonsoft.Json;

namespace astute.Models
{
    public class Suzy_Error_Model
    {
        public string Status { get; set; }

        public string Message { get; set; }
    }
    public class Suzy_Model
    {
        [JsonProperty("Stock #")]
        public string StockNumber { get; set; }

        public string Availability { get; set; }

        public string Shape { get; set; }

        public double Weight { get; set; }

        public string Color { get; set; }

        public string Clarity { get; set; }

        [JsonProperty("Cut Grade")]
        public string CutGrade { get; set; }

        public string Polish { get; set; }

        public string Symmetry { get; set; }

        [JsonProperty("Fluorescence Intensity")]
        public string FluorescenceIntensity { get; set; }

        [JsonProperty("Fluorescence Color")]
        public string FluorescenceColor { get; set; }

        public string Measurements { get; set; }

        public string Lab { get; set; }

        [JsonProperty("Certificate #")]
        public string CertificateNumber { get; set; }

        public double Rapprice { get; set; }

        public double RapNetPrice { get; set; }

        [JsonProperty("Total Amount")]
        public double TotalAmount { get; set; }

        [JsonProperty("Fancy Color")]
        public string FancyColor { get; set; }

        [JsonProperty("Fancy Color Intensity")]
        public string FancyColorIntensity { get; set; }

        [JsonProperty("Fancy Color Overtone")]
        public string FancyColorOvertone { get; set; }

        [JsonProperty("Depth %")]
        public double DepthPercentage { get; set; }

        [JsonProperty("Table %")]
        public double TablePercentage { get; set; }

        [JsonProperty("GirdleThin")]
        public string GirdleThin { get; set; }

        [JsonProperty("GirdleThick")]
        public string GirdleThick { get; set; }

        [JsonProperty("Girdle %")]
        public double GirdlePercentage { get; set; }

        [JsonProperty("Girdle Condition")]
        public string GirdleCondition { get; set; }

        [JsonProperty("Culet Size")]
        public string CuletSize { get; set; }

        [JsonProperty("Culet Condition")]
        public string CuletCondition { get; set; }

        [JsonProperty("Crown Height")]
        public double CrownHeight { get; set; }

        [JsonProperty("Crown Angle")]
        public double CrownAngle { get; set; }

        [JsonProperty("Pavilion Angle")]
        public double PavilionAngle { get; set; }

        [JsonProperty("Pavilion Depth")]
        public double PavilionDepth { get; set; }

        [JsonProperty("Laser Inscription")]
        public string LaserInscription { get; set; }

        [JsonProperty("Cert Comment")]
        public string CertComment { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        public string City { get; set; }

        [JsonProperty("Is Matched Pair Separable")]
        public string IsMatchedPairSeparable { get; set; }

        [JsonProperty("Allow Raplink Feed")]
        public string AllowRaplinkFeed { get; set; }

        [JsonProperty("Key To Symbols")]
        public string KeyToSymbols { get; set; }

        public string Milky { get; set; }

        public string Shade { get; set; }

        [JsonProperty("Star Length")]
        public string StarLength { get; set; }

        [JsonProperty("Diamond Image")]
        public string DiamondImage { get; set; }

        [JsonProperty("Member Comments")]
        public string MemberComments { get; set; }

        [JsonProperty("TRADE SHOW")]
        public string TradeShow { get; set; }
    }
}
