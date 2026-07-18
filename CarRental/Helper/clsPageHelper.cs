using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Helper
{
    public class clsPageHelper
    {
        private readonly frmMain frmMain;
        public clsPageHelper(frmMain main)
        {
            this.frmMain = main;
        }

        public void SetPage(UserControl pageUserControl)
        {
            var oldPage = frmMain.pnlMain.Controls.OfType<UserControl>().FirstOrDefault();

            if (oldPage != null && oldPage != pageUserControl)
            {
                frmMain.pnlMain.Controls.Remove(oldPage);
            }

            if (oldPage != pageUserControl)
            {
                pageUserControl.Dock = DockStyle.Fill;
                frmMain.pnlMain.Controls.Add(pageUserControl);
            }
        }
    }
}
