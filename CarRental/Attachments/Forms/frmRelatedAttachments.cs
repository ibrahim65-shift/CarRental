using CarRental.Attachments.Controls;
using CarRental.Helper;
using CarRental.Reports.RentalReports.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Attachments.Forms
{
    public partial class frmRelatedAttachments : Form
    {
        
        public frmRelatedAttachments(string relatedTable, int relatedId, string attachOwner)
        {
            InitializeComponent();

            var ctrl = clsPageManager.GetPage<ctrlRelatedAttachments, frmRelatedAttachments>
                            (this, r => new ctrlRelatedAttachments(relatedTable, relatedId, attachOwner));

            this.Controls.Add(ctrl);
            ctrl.Dock = DockStyle.Fill;
        }
    }
}
