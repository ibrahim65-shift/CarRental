using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Helper
{
    public static class clsMessages
    {
        public static void ShowSuccess(string message = "تمت العملية بنجاح.")
        {
            MessageBox.Show(message, "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void ShowError(string message = "حدث خطأ غير متوقع. يرجى المحاولة لاحقًا.")
        {
            MessageBox.Show(message, "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowWarning(string message = "تنبيه: يرجى التحقق من البيانات.")
        {
            MessageBox.Show(message, "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void ShowInfo(string message = "معلومة: يرجى الاطلاع.")
        {
            MessageBox.Show(message, "معلومة", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool ShowDeleteDialog()
        {
            var result = MessageBox.Show("هل أنت متأكد من رغبتك في الحذف ؟", "حذف",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
