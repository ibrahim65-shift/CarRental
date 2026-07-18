using CarRental.Customers.People.Forms;
using CarRental.Helper;
using CarRental_Buisness.Models.People;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Customers;
using CarRental_Buisness.Services.People;
using SharedClass;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Customers.People.Controls
{
    public partial class ctrPersonCardWithFilter : UserControl
    {
        private enum enFilterType { PersonID=0, NationalNo=1 }

        private readonly clsPersonService _personService;

        public Func<int , Task<bool>> ExistsValidator { get; set; }

        private bool _isSearching;

        public bool SearchPanelEnabled 
        {
            get => pnlSearch.Enabled;
            set => pnlSearch.Enabled = value;
        }
        public clsPersonDto SelectedPerson => ctrlPersonCard1.Person;
        public ctrPersonCardWithFilter()
        {
            InitializeComponent();

            _personService = new clsPersonService();

            cbFilter.SelectedIndex = (int)enFilterType.PersonID;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (_isSearching)
                return;

            try
            {
                _isSearching = true;
                btnSearch.Enabled = false;

                errorProvider1.Clear();

                string input = numericTextBoxSearch.Text.Trim();

                if (string.IsNullOrWhiteSpace(input))
                {
                    errorProvider1.SetError(numericTextBoxSearch, "الرجاء إدخال قيمة للبحث");
                    return;
                }

                errorProvider1.SetError(numericTextBoxSearch, null);

                bool success ;

                if ((enFilterType)cbFilter.SelectedIndex == enFilterType.PersonID)
                {
                    if(!int.TryParse(input , out int personId))
                    {
                        errorProvider1.SetError(numericTextBoxSearch, "رقم الشخص غير صالح");
                        return;
                    }

                    success = await SearchByPersonIDAsync(personId);
                }
                else
                {
                    success = await SearchByNationalIDAsync(input);
                }

                btnAddPerson.Enabled = !success;

                if (!success)
                {
                    clsMessages.ShowError("تعذر تحميل بيانات الشخص");
                }
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrPersonCardWithFilter.btnSearch_Click", ex);
                MessageBox.Show("حدث خطأ غير متوقع أثناء تنفيذ عملية البحث");
            }
            finally
            {
                btnSearch.Enabled = true;
                _isSearching = false;
            }
        }
        private void numericTextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            lblSearch.Visible = string.IsNullOrEmpty(numericTextBoxSearch.Text);
            errorProvider1.Clear();
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResetControl();
        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            using (var frm = new frmAddEditPerson(_personService))
            { 
                if(frm.ShowDialog()==DialogResult.OK)
                {
                    numericTextBoxSearch.Clear();
                    ctrlPersonCard1.ResetPersonInfo();
                }
            }

        }
        public async Task<bool> SearchByPersonIDAsync(int personId )
        {
            if(cbFilter.SelectedIndex != (int)enFilterType.PersonID)
            {
                cbFilter.SelectedIndex = (int)enFilterType.PersonID;
            }
            numericTextBoxSearch.Text = personId.ToString();

            if (personId<=0)
            {
                errorProvider1.SetError(numericTextBoxSearch, "رقم الشخص غير صالح");

                ctrlPersonCard1.ResetPersonInfo();
                return false;
            }

            if (!await _ValidateSelectionByPersonIDAsync(personId))
                return false;

           return await ctrlPersonCard1.LoadAsync(() =>_personService.GetPersonByIDAsync(personId));
        }
        public async Task<bool> SearchByNationalIDAsync(string nationalNo)
        {
            if (cbFilter.SelectedIndex != (int)enFilterType.NationalNo)
            {
                cbFilter.SelectedIndex = (int)enFilterType.NationalNo;
            }
            numericTextBoxSearch.Text = nationalNo;

            if (string.IsNullOrWhiteSpace(nationalNo))
            {
                errorProvider1.SetError(numericTextBoxSearch, "الرجاء إدخال رقم الهوية");

                ctrlPersonCard1.ResetPersonInfo();
                return false;
            }

            var personResult = await _personService.GetPersonByNationalNoAsync(nationalNo);

            if(!personResult.Success || personResult.Data == null)
            {
                ctrlPersonCard1.ResetPersonInfo();
                return false;
            }

            if (! await _ValidateSelectionByPersonIDAsync(personResult.Data.PersonID))
                return false;

            return ctrlPersonCard1.LoadData(personResult.Data);
        }
        public void ResetControl()
        {
            numericTextBoxSearch.Clear();
            errorProvider1.Clear();

            btnAddPerson.Enabled = true;

            ctrlPersonCard1.ResetPersonInfo();
        }
        private async Task<bool> _ValidateSelectionByPersonIDAsync(int personId)
        {
            if (ExistsValidator == null)
                return true;

            bool exists = await ExistsValidator(personId);

            if(exists)
            {
                ctrlPersonCard1.ResetPersonInfo();
                return false;
            }

            return true;
        }

    }
}
