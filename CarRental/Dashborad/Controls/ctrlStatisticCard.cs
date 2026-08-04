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
    public partial class ctrlStatisticCard : UserControl
    {
        public ctrlStatisticCard()
        {
            InitializeComponent();
        }

        public string TitleText
        {
            get => lblTitle.Text;
            set=> lblTitle.Text = value;
        }
        public Image TitleImage
        {
            get => lblTitle.Image;
            set => lblTitle.Image = value;
        }

        public void SetValue<T>(T value)
        {
            lblValue.Text = $"{value:N0}";
        }
    }
}
