using System;
using System.ComponentModel.DataAnnotations;

namespace astute.Models
{
    public partial class UserModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
