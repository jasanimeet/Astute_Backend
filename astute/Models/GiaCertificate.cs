using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace astute.Models
{
    public class GiaResponse
    {
        [JsonProperty("statusCode")] // or JsonProperty depending on the serializer
        public string StatusCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public List<GiaCertificate> Data { get; set; }
    }
    public class GiaCertificate
    {
        [JsonProperty("CERTIFICATE NO")]
        public string CertificateNo { get; set; }

        [JsonProperty("CERTIFICATE DATE")]
        public string CertificateDate { get; set; }

        [JsonProperty("SHAPE")]
        public string Shape { get; set; }

        [JsonProperty("CTS")]
        public double CTS { get; set; }

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

        [JsonProperty("FLS INTENSITY")]
        public string FlsIntensity { get; set; }

        [JsonProperty("LENGTH")]
        public double Length { get; set; }

        [JsonProperty("WIDTH")]
        public double Width { get; set; }

        [JsonProperty("DEPTH")]
        public double Depth { get; set; }

        [JsonProperty("DEPTH PER")]
        public double DepthPer { get; set; }


        [JsonProperty("TABLE PER")]
        public double TablePer { get; set; }


        [JsonProperty("CULET")]
        public string Culet { get; set; }


        [JsonProperty("LASER INSCRIPTION")]
        public string LaserInscription { get; set; }


        [JsonProperty("CROWN ANGLE")]
        public double? CrownAngle { get; set; }


        [JsonProperty("CROWN HEIGHT")]
        public double? CrownHeight { get; set; }


        [JsonProperty("PAVILION ANGLE")]
        public double? PavilionAngle { get; set; }


        [JsonProperty("PAVILION HEIGHT")]
        public double? PavilionHeight { get; set; }


        [JsonProperty("GIRDLE PER")]
        public double? GirdlePer { get; set; }

        [JsonProperty("GIRDLE CONDITION")]
        public string GirdleCondition { get; set; }

        [JsonProperty("LR HALF")]
        public double? LRHalf { get; set; }

        [JsonProperty("STAR LN")]
        public double? StarLn { get; set; }

        [JsonProperty("CERTIFICATE LINK")]
        public string CertificateLink { get; set; }

        [JsonProperty("KEY TO SYMBOL")]
        public string KeyToSymbol { get; set; }

        [JsonProperty("GIA COMMENTS")]
        public string GiaComments { get; set; }

        [JsonProperty("RAP RATE")]
        public double? RapRate { get; set; }

        [JsonProperty("RAP AMOUNT")]
        public double? RapAmount { get; set; }

        [JsonProperty("Supplier_Name")]
        public string SupplierName { get; set; }

        [JsonProperty("Customer_Name")]
        public string CustomerName { get; set; }

    } 
}