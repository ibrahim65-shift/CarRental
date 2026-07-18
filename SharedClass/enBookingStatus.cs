using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass
{
    public enum enBookingStatus
    {
        [Description("مسودة")]
        Draft = 1,

        [Description("مؤكد")]
        Confirmed = 2,

        [Description("نشط")]
        Active = 3,

        [Description("مكتمل")]
        Completed = 4,

        [Description("ملغي")]
        Cancelled = 5,

        [Description("عدم حضور")]
        NoShow = 6
    }
}
