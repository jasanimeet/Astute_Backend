using Newtonsoft.Json;

namespace astute.Models
{
    public class Lab_Entry_Status
    {
        [JsonProperty("Id")]
        public int? Id { get; set; }

        [JsonProperty("STATUS")]
        public string? Status { get; set; }

        [JsonProperty("BASE DISC")]
        public string? Supp_Base_Disc { get; set; }

        [JsonProperty("BASE AMOUNT")]
        public string? Supp_Base_Amt { get; set; }

        [JsonProperty("COST DISC")]
        public string? Supp_Cost_Disc { get; set; }

        [JsonProperty("COST AMOUNT")]
        public string? Supp_Cost_Amt { get; set; }
        
        [JsonProperty("OFFER DISC")]
        public string? Final_Disc { get; set; }

        [JsonProperty("OFFER AMOUNT")]
        public string? Final_Amt { get; set; }
                
        [JsonProperty("Supplier_Short_Name")]
        public string? Supplier_Short_Name { get; set; }

        [JsonProperty("BGM")]
        public string? BGM { get; set; }

        [JsonProperty("TABLE BLACK")]
        public string? Table_Black { get; set; }

        [JsonProperty("SIDE BLACK")]
        public string? Crown_Black { get; set; }

        [JsonProperty("TABLE WHITE")]
        public string? Table_White { get; set; }

        [JsonProperty("SIDE WHITE")]
        public string? Crown_White { get; set; }

        [JsonProperty("TABLE OPEN")]
        public string? Table_Open { get; set; }

        [JsonProperty("CROWN OPEN")]
        public string? Crown_Open { get; set; }

        [JsonProperty("PAVILION OPEN")]
        public string? Pavilion_Open { get; set; }

        [JsonProperty("Girdle Open")]
        public string? Girdle_Open { get; set; }

        [JsonProperty("BGM_Id")]
        public int? BGM_Id { get; set; }

        [JsonProperty("Table_Black_Id")]
        public int? Table_Black_Id { get; set; }

        [JsonProperty("Crown_Black_Id")]
        public int? Crown_Black_Id { get; set; }

        [JsonProperty("Table_White_Id")]
        public int? Table_White_Id { get; set; }

        [JsonProperty("Crown_White_Id")]
        public int? Crown_White_Id { get; set; }

        [JsonProperty("Table_Open_Id")]
        public int? Table_Open_Id { get; set; }

        [JsonProperty("Crown_Open_Id")]
        public int? Crown_Open_Id { get; set; }

        [JsonProperty("Pavilion_Open_Id")]
        public int? Pavilion_Open_Id { get; set; }

        [JsonProperty("Girdle_Open_Id")]
        public int? Girdle_Open_Id { get; set; }
    }
    public class Lab_Entry_Status_List 
    {
        public object? Lab_Entry_Status { get; set; }
    }
}
