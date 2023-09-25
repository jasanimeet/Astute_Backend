using Microsoft.EntityFrameworkCore.Storage;
using OfficeOpenXml.Drawing.Chart.Style;
using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Security.Cryptography;

namespace astute.Models
{
    public partial class Temp_Stock
    {
        [Key]
        public int iId { get; set; }
        public string? sRefNo { get; set; }
        public string? sShape { get; set; }
        public string? sCertiNo { get; set; }
        public string? sPointer { get; set; }
        public string? sColor { get; set; }
        public string? sClarity { get; set; }
        public float dCts { get; set; }
        public double? dRepPrice { get; set; }
        public float? dDisc { get; set; }
        public string? sCut { get; set; }
        public string? sPolish { get; set; }
        public string? sSymm { get; set; }
        public string? sFls { get; set; }
        public float? dLength { get; set; }
        public float? dWidth { get; set; }
        public float? dDepth { get; set; }
        public float? dDepthPer { get; set; }
        public float? dTablePer { get; set; }
        public string? sStatus { get; set; }
        public string? sLab { get; set; }
        public float? dCrAng { get; set; }
        public float? dCrHt { get; set; }
        public float? dPavAng { get; set; }
        public float? dPavHt { get; set; }
        public string? sGirdle { get; set; }
        public string? sShade { get; set; }
        public decimal? iSeqNo { get; set; }
        public string? sInclusion { get; set; }
        public string? sTableNatts { get; set; }
        public string? sSideNatts { get; set; }
        public string? sCulet { get; set; }
        public float? dTableDepth { get; set; }
        public string? sHNA { get; set; }
        public string? sSideFtr { get; set; }
        public string? sTableFtr { get; set; }
        public string? sComments { get; set; }
        public string? sSymbol { get; set; }
        public float? dDiscByDate { get; set; }
        public string? sStoneClarity { get; set; }
        public string? sLuster { get; set; }
        public string? sInscription { get; set; }
        public string? sStrLn { get; set; }
        public string? sLrHalf { get; set; }
        public float? dGirdlePer { get; set; }
        public string? sGirdleType { get; set; }
        public string? sOpen { get; set; }
        public string? sReviseDiscFlag { get; set; }
        public string? sPartyName { get; set; }
        public string? sCrownInclusion { get; set; }
        public string? sCrownNatts { get; set; }
        public DateTime? dCertiDate { get; set; }
        public bool? Upcoming_Flag { get; set; }
        public string? sImglink { get; set; }
        public string? sVdoLink { get; set; }
        public string? SegomaImg { get; set; }
        public string? SegomaVdo { get; set; }
        public bool SOffer { get; set; }
        public  bool Overseas_Flag { get; set; }
        public string? Supplier_Stone_No { get; set; }
        public string? Supplier { get; set; }
        public string? Location { get; set; }
        public string? Certi_Path { get; set; }
        public bool? MP4_Status { get; set; }
        public bool? JSON_Status { get; set; }
        public string? BGM { get; set; }
        public Byte? iShapeSr { get; set; }
        public Byte? iColorSr { get; set; }
        public Byte? iClaritySr { get; set; }
        public Byte? iCutSr { get; set; }
        public Byte? iPolishSr { get; set; }
        public Byte? iSymmSr { get; set; }
        public Byte? iFlsSr { get; set; }
        public bool? bImage { get; set; }
        public bool? bHDmovie { get; set; }
        public int? iSupplId { get; set; }
        public float? sSupplDisc { get; set; }
        public string? sSupplPrefix { get; set; }
        public int? iSupplLocation { get; set; }
        public string? sLrHgt { get; set; }
        public string? sEyeClean { get; set; }
        public int? iStockId { get; set; }
        public string? Promotion { get; set; }
        public int? SOffer_Validity { get; set; }
        public DateTime? OfferDate { get; set; }
        public bool? bPRimg { get; set; }
        public bool? bASimg { get; set; }
        public bool? bHTimg { get; set; }
        public bool? bHBimg { get; set; }
        public DateTime? TransDate { get; set; }
        public string? ColorType { get; set; }
        public decimal? Fancy_Amount { get; set; }
        public int? Hold_Party_Code { get; set; }
        public string? sImglink1 { get; set; }
        public string? sImglink2 { get; set; }
        public string? sImglink3 { get; set; }
        public int? Assist_By { get; set; }
        public string? Hold_By { get; set; }
        public string? Table_Open { get; set; }
        public string? Crown_Open { get; set; }
        public string? Pav_Open { get; set; }
        public string? Girdle_Open { get; set; }
        public string? Cur_Status { get; set; }
        public string? HoldDateTime { get; set; }
        public string? No_OS_sImglink { get; set; }
        public string? Overseas_Image_Download_Link { get; set; }
        public string? Overseas_Image_Download_Link1 { get; set; }
        public string? Overseas_Image_Download_Link2 { get; set; }
        public string? Overseas_Image_Download_Link3 { get; set; }
        public string? Overseas_Certi_Download_Link { get; set; }
        public bool? ImageStatus { get; set; }
        public bool? VideoStatus { get; set; }
        public Byte? iPointerSr { get; set; }
        public string? certi_type { get; set; }
        public decimal? RATIO { get; set; }
        public string? CertiTypeLink { get; set; }
    }
}
