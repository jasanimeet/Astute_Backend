using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public class Party_Url_Format
    {
        [Key]
        public int Id { get; set; }
        public int Supplier_Id { get; set; }
        public string? Party_Name { get; set; }
        public string? Sort_Code { get; set; }
        public string? Image_Link { get; set; }
        public string? Video_Link { get; set; }
        public string? Cert_Link { get; set; }
        public bool? Is_Ref_Split_W { get; set; }
        public bool? Is_Video_Iframe { get; set; }
        public int? Is_Img_Cert { get; set; }
        public int? Is_Video_Cert { get; set; }
        public int? Is_Cert_Cert { get; set; }
    }
}
