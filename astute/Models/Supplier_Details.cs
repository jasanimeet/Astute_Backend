using Microsoft.AspNetCore.Http;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class Supplier_Details
    {
        [Key]
        public int Party_Id { get; set; }
        //public int API_Id { get; set; }
        //public string? API_URL { get; set; }
        //public string? API_User { get; set; }
        //public string? API_Password { get; set; }
        //public string? API_Method { get; set; }
        //public string? API_Response { get; set; }
        //public bool? API_Status { get; set; }
        //public bool? API_Disc_Inverse { get; set; }
        //public bool? API_Auto_Ref_No { get; set; }
        //public string? API_RepeateveryType { get; set; }
        //public string? API_Repeatevery { get; set; }
        //public bool? API_Lab { get; set; }
        //public bool? API_Overseas { get; set; }
        //public int FTP_Id { get; set; }
        //public string? Host { get; set; }
        //public Int16? Ftp_Port { get; set; }
        //public string? Ftp_User { get; set; }
        //public string? Ftp_Password { get; set; }
        //public string? Ftp_File_Name { get; set; }
        //public string? Ftp_File_Type { get; set; }
        //public bool? FTP_Disc_Inverse { get; set; }
        //public bool? FTP_Auto_Ref_No { get; set; }
        //public string? FTP_RepeateveryType { get; set; }
        //public string? FTP_Repeatevery { get; set; }
        //public bool? FTP_Lab { get; set; }
        //public bool? FTP_Overseas { get; set; }
        //public int File_Id { get; set; }
        //public string? File_Location { get; set; }
        //public string? File_Type { get; set; }
        //public string? Offer_Name { get; set; }
        //public bool? File_Disc_Inverse { get; set; }
        //public bool? File_Auto_Ref_No { get; set; }
        //public string? File_Location_1 { get; set; }
        //public string? File_Type_1 { get; set; }
        //public string? File_Location_2 { get; set; }
        //public string? File_Type_2 { get; set; }
        //public string? File_RepeateveryType { get; set; }
        //public string? File_Repeatevery { get; set; }
        public Party_Api Party_Api { get; set; }
        public Party_FTP Party_FTP { get; set; }
        public Party_File Party_File { get; set; }
        [NotMapped]
        public IList<Supplier_Column_Mapping> Supplier_Column_Mapping_List { get; set; } = new List<Supplier_Column_Mapping>();
    }
}
