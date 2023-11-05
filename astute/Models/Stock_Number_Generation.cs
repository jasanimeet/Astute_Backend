namespace astute.Models
{

    public partial class Stock_Number_Generation
    {
        public int Id { get; set; }
        public string? Exc_Party_Id { get; set; } = string.Empty;
        public string? Pointer_Id { get; set; } = string.Empty;
        public string? Shape { get; set; } = string.Empty;
        public string? Shape_Group { get; set; } = string.Empty;
        public string Stock_Type { get; set; } = string.Empty;
        public string Front_Prefix { get; set; } = string.Empty;
        public string? Back_Prefix { get; set; } = string.Empty;
        public string? Front_Prefix_Alloted { get; set; } = string.Empty;
        public int Start_Format { get; set; }
        public int End_Format { get; set; }
        public int Start_Number { get; set; }
        public int End_Number { get; set; }
        public string? Live_Prefix { get; set; } = string.Empty;

    }
}
