using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass
{
    public enum enInsuranceType
    {
        [Description("شامل")]
        Comprehensive = 1 ,

        [Description("ضد الغير")]
        ThirdParty=2
    }
}
