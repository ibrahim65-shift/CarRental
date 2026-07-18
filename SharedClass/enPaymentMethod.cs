using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClass
{
    public enum enPaymentMethod
    {
        [Description("كاش")]
        Cash = 1,

        [Description("بطاقة ائتمان")]
        CreditCard = 2,

        [Description("بطاقة خصم")]
        DebitCard = 3,

        [Description("تحويل بنكي")]
        BankTransfer = 4,

        [Description("دفع عبر الهاتف")]
        MobilePayment = 5
    }
}
