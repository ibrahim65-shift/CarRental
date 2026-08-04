using CarRental.Properties;
using CarRental_Buisness.Models.Dashboard;
using SharedClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Dashborad.Controls
{
    public partial class ctrlAlertCard : UserControl
    {
        public ctrlAlertCard()
        {
            InitializeComponent();
        }

        public void LoadData(clsAlertsDto alert)
        {
            lblTitle.Text = alert.Title;
            lblDescription.Text = alert.Description;
            lblReferenceID.Text = $"#{alert.ReferenceID}";

            switch(alert.Severity)
            {
                case enSeverityType.Info:
                    pbSeverity.Image = Resources.GoldCircle_32;
                    break;

                case enSeverityType.Warning:
                    pbSeverity.Image = Resources.orangeCircle_32;
                    break;

                case enSeverityType.Critical:
                    pbSeverity.Image = Resources.Redcircle_32;
                    break;
            }
        }
    }
}
