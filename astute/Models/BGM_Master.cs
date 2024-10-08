﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace astute.Models
{
    public partial class BGM_Master
    {
        [Key]
        public int Bgm_Id { get; set; }
        public string? BGM { get; set; }
        public Int16? Sort_No { get; set; }
        public Int16? Order_No { get; set; }
        public bool? Status { get; set; }
        [NotMapped]
        public IList<BGM_Detail> BGM_Detail_List { get; set; }
    }
}
