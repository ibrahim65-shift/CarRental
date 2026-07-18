using CarRental.Properties;
using CarRental_Buisness.Models.People;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.People;
using SharedClass;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Customers.People.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        private const string _Unknown = "غير معروف";

        private clsPersonDto _person;
        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public clsPersonDto Person =>_person;
        public bool LoadData(clsPersonDto person)
        {
            if (person == null)
            {
                ResetPersonInfo();
                return false;
            }

            _FillPersonInfo(person);
            return true;
        }
        public async Task<bool> LoadAsync(Func<Task<clsServiceResult<clsPersonDto>>> loader)
        {
            if (loader == null)
                throw new ArgumentException(nameof(loader));

            try
            {
                var result = await loader();

                if(!result.Success || result.Data==null)
                {
                    ResetPersonInfo();
                    return false;  
                }

                _FillPersonInfo(result.Data);
                return true;
            }
            catch(Exception ex) 
            {
                clsEventLogger.LogException("ctrlPersonCard.LoadAsync", ex);
                ResetPersonInfo();
                return false;
            }
        }
        private void _FillPersonInfo(clsPersonDto person)
        {
            _person = person;

            lblPersonID.Text    = person.PersonID.ToString();
            lblNationalNo.Text  = person.NationalNo;
            lblFirstName.Text   = person.FirstName;
            lblSecondName.Text  = person.SecondName;
            lblThirdName.Text = string.IsNullOrWhiteSpace(person.ThirdName) ? _Unknown : person.ThirdName;
            lblLastName.Text    = person.LastName;
            lblBirthDate.Text   = person.BirthDate.ToString("dd/MM/yyyy") ?? _Unknown;

            bool isMale = person.Gender == enGenderType.Male;
            lblGender.Text      = isMale ? "ذكر" : "أنثى";
            lblGenderImage.Image = isMale ? Resources.male_32 : Resources.female32;

            lblPhone.Text       = string.IsNullOrWhiteSpace(person.Phone) ? _Unknown : person.Phone;
            lblEmail.Text       = string.IsNullOrWhiteSpace(person.Email) ? _Unknown : person.Email;
            lblAddress.Text     = string.IsNullOrWhiteSpace(person.Address) ? _Unknown : person.Address;
        }
        public void ResetPersonInfo()
        {
            _person = null;

            lblPersonID.Text   = "????";
            lblNationalNo.Text  = "????";
            lblFirstName.Text = "????";
            lblSecondName.Text  = "????";
            lblThirdName.Text   = "????";
            lblLastName.Text = "????";
            lblBirthDate.Text  = "????";
            lblGender.Text     = "????";
            lblPhone.Text      = "????";
            lblEmail.Text      = "????";
            lblAddress.Text    = "????";
        }
    }
}
