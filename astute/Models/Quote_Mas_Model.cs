using Microsoft.EntityFrameworkCore;

namespace astute.Models
{
    [Keyless]
    public partial class Quote_Mas_Model
    {
        public string Quote { get; set; }
        public string Author { get; set; }
    }
}
