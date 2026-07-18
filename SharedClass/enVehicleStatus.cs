using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass
{
    public enum enVehicleStatus
    {
        [Description("متاح")]
        Available = 1,

        [Description("الصيانة")]
        Maintenance = 2,

        [Description("خارج الخدمة")]
        OutofService = 3,

        [Description("مؤجر")]
        Rented = 4,

        [Description("محجوز")]
        Reserved = 5
    }
}
