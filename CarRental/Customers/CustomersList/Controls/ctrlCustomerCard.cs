using CarRental.Helper;
using CarRental.Properties;
using CarRental_Buisness.Models.Customers;
using CarRental_Buisness.Models.People;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Customers;
using CarRental_Buisness.Services.People;
using CarRental_Buisness.Services.Users;
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

namespace CarRental.Customers.CustomersList.Controls
{
    public partial class ctrlCustomerCard : UserControl
    {
        private const string _Unknown = "غير معروف";

        private clsCustomerDto _customer;
        public ctrlCustomerCard()
        {
            InitializeComponent();
        }

        public clsCustomerDto Customer =>_customer;
        public bool LoadData(clsCustomerDto customer)
        {
            if(customer == null)
            {
                ResetCustomerInfo();
                return false;
            }

            _FillCustomerInfo(customer);
            return true;
        }
        public async Task<bool> LoadAsync(Func<Task<clsServiceResult<clsCustomerDto>>> loader)
        {
            if (loader == null)
                throw new ArgumentException(nameof(loader));

            try
            {
                var result = await loader();

                if(!result.Success || result.Data==null)
                {
                    ResetCustomerInfo();
                    return false;  
                }

                _FillCustomerInfo(result.Data);
                return true;
            }
            catch(Exception ex) 
            {
                clsEventLogger.LogException("ctrlCustomerCardLoadAsync", ex);

                ResetCustomerInfo();
                return false;
            }
        }
        private void _FillCustomerInfo(clsCustomerDto customer)
        {
            _customer = customer;
            lblCustomerID.Text    = customer.CustomerID.ToString();
            lblDriverLicenseNumber.Text  = customer.DriverLicenseNumber;
            lblNationalNo.Text   = customer.PersonInfo.NationalNo;
            lblFirstName.Text  = customer.PersonInfo.FirstName;
            lblSecondName.Text = customer.PersonInfo.SecondName;
            lblThirdName.Text    = customer.PersonInfo.ThirdName ?? _Unknown;
            lblLastName.Text = customer.PersonInfo.LastName;
            lblBirthDate.Text   = customer.PersonInfo.BirthDate.ToString("dd/MM/yyyy") ?? _Unknown;

            bool isMale = customer.PersonInfo.Gender == enGenderType.Male ? true : false;
            lblGender.Text      = isMale ? "ذكر" : "أنثى";
            lblGenderImage.Image = isMale ? Resources.male_32 : Resources.female32;

            lblLicenseExpiration.Text       = customer.DriverLicenseExpiry.ToString("dd/MM/yyyy");
            lblPhone.Text       = customer.PersonInfo.Phone ?? _Unknown;
            lblEmail.Text     = customer.PersonInfo.Email?? _Unknown;
            lblAddress.Text     = customer.PersonInfo.Address?? _Unknown;
        }
        public void ResetCustomerInfo()
        {
            _customer = null;

            lblCustomerID.Text   = "????";
            lblDriverLicenseNumber.Text  = "????";
            lblNationalNo.Text = "????";
            lblFirstName.Text  = "????";
            lblSecondName.Text   = "????";
            lblThirdName.Text = "????";
            lblLastName.Text = "????";
            lblBirthDate.Text  = "????";
            lblLicenseExpiration.Text     = "????";
            lblGender.Text      = "????";
            lblPhone.Text      = "????";
            lblEmail.Text    = "????";
            lblAddress.Text = "????";
        }
    }
}
