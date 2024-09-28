using System.Collections.Generic;

namespace astute.Models
{
    public partial class RapaportFilterModel
    {
        public IList<Rapaport_Clarity_Value> Rapaport_Clarity_Value { get; set; } = new List<Rapaport_Clarity_Value>();
        public IList<Rapaport_Color_Value> Rapaport_Color_Value { get; set; } = new List<Rapaport_Color_Value>();
        public IList<Rapaport_Date_Value> Rapaport_Date_Values { get; set; } = new List<Rapaport_Date_Value>();
    }
}
