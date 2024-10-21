using Newtonsoft.Json;
using System.Collections.Generic;

namespace astute.Models
{
    public class Sunrise_Model
    {
        public string? DNA { get; set; }
        public string? ViewImage { get; set; }
        public string? ViewVideo { get; set; }
        public string? StockID { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; }
        public string? Shape { get; set; }
        public string? Pointer { get; set; }
        public string? Lab { get; set; }
        public string? CertiNo { get; set; }
        public string? Bgm { get; set; }
        public string? Color { get; set; }
        public string? Clarity { get; set; }
        public string? Cts { get; set; }
        public string? RapPrice { get; set; }
        public string? RapAmt { get; set; }
        public string? Disc { get; set; }
        public string? NetAmt { get; set; }
        public string? FinalDisc { get; set; }
        public string? FinalValue { get; set; }
        public string? PPC { get; set; }
        public string? Cut { get; set; }
        public string? Polish { get; set; }
        public string? Symm { get; set; }
        public string? Fls { get; set; }
        public string? Length { get; set; }
        public string? Width { get; set; }
        public string? Depth { get; set; }
        public string? DepthPer { get; set; }
        public string? TablePer { get; set; }
        public string? KeyToSymbol { get; set; }
        public string? Culet { get; set; }
        public string? TableBlack { get; set; }
        public string? CrownBlack { get; set; }
        public string? TableWhite { get; set; }
        public string? CrownWhite { get; set; }
        public string? CrAng { get; set; }
        public string? CrHt { get; set; }
        public string? PavAng { get; set; }
        public string? PavHt { get; set; }
        public string? Girdle { get; set; }
        public string? GirdleType { get; set; }
        public string? LabLink { get; set; }
    }
    public class Sunrise_LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string grant_type { get; set; }
    }
    public class Sunrise_LoginResponse
    {
        public string access_token { get; set; }
    }

    public class Sunrise_Stock_Model
    {
        public string sRefNo { get; set; }
        public string sShape { get; set; }
        public string sCertiNo { get; set; }
        public string sColor { get; set; }
        public string sClarity { get; set; }
        public float dCts { get; set; }
        public float dDisc { get; set; }
        public float dAmt { get; set; }
        public string sCut { get; set; }
        public string sPolish { get; set; }
        public string sSymm { get; set; }
        public string sFls { get; set; }
        public float dLength { get; set; }
        public float dWidth { get; set; }
        public float dDepth { get; set; }
        public float dDepthPer { get; set; }
        public float dTablePer { get; set; }
        public object sStatus { get; set; }
        public string sLab { get; set; }
        public float dCrAng { get; set; }
        public float dCrHt { get; set; }
        public float dPavAng { get; set; }
        public float dPavHt { get; set; }
        public string sGirdle { get; set; }
        public string sShade { get; set; }
        public string sTableNatts { get; set; }
        public object sSideNatts { get; set; }
        public string sCulet { get; set; }
        public object sComments { get; set; }
        public string sSymbol { get; set; }
        public string sLuster { get; set; }
        public string sInscription { get; set; }
        public float sStrLn { get; set; }
        public float sLrHalf { get; set; }
        public float dGirdlePer { get; set; }
        public string sGirdleType { get; set; }
        public object sCrownInclusion { get; set; }
        public string sCrownNatts { get; set; }
        public string sImglink { get; set; }
        public string sVdoLink { get; set; }
        public string Location { get; set; }
        public string Certi_Path { get; set; }
        public string BGM { get; set; }
        public string sImglink1 { get; set; }
        public object sImglink2 { get; set; }
        public object sImglink3 { get; set; }
        public object Table_Open { get; set; }
        public object Crown_Open { get; set; }
        public object Pav_Open { get; set; }
        public object Girdle_Open { get; set; }
        public string Cur_Status { get; set; }
        public object certi_type { get; set; }
        public object certitype_path { get; set; }
    }
    public class StockResponse
    {
        [JsonProperty("data")]
        public List<Sunrise_Stock_Model> Data { get; set; }
    }
    public class SunriseStockResponse
    {
        [JsonProperty("data")]
        public List<Sunrise_Stock_Model> data { get; set; }
    }
}
