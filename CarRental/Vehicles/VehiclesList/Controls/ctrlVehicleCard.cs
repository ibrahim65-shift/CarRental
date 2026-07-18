using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Vehicles;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Vehicles;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2023.M02.Main;
using DocumentFormat.OpenXml.Office2013.Excel;
using SharedClass;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Vehicles.VehiclesList.Controls
{
    public partial class ctrlVehicleCard : UserControl
    {
        private const string _Unknown = "غير معروف";

        private clsVehicleDto _vehicle;
        public ctrlVehicleCard()
        {
            InitializeComponent();
        }

        public clsVehicleDto Vehicle =>_vehicle;
        public bool LoadData(clsVehicleDto vehicle)
        {
            if(vehicle==null)
            {
                ResetVehicleInfo();
                return false;
            }

             _FillVehicleInfo(vehicle); 
            return true;
        }
        public async Task<bool> LoadAsync(Func<Task<clsServiceResult<clsVehicleDto>>> loader)
        {
            if(loader==null)
                throw new ArgumentException(nameof(loader));

            try
            {
                var result = await loader();

                if(!result.Success || result.Data==null)
                {
                    ResetVehicleInfo();
                    return false;  
                }

                _FillVehicleInfo(result.Data);
                return true;
            }
            catch(Exception ex)
            {
                clsEventLogger.LogException("ctrlVehicleCard.LoadAsync", ex);

                ResetVehicleInfo();
                return false;
            }
        }
        private void _FillVehicleInfo(clsVehicleDto vehicle)
        {
            _vehicle = vehicle;

            lblVehicleID.Text = vehicle.VehicleID.ToString();
            lblMake.Text = vehicle.MakeName;
            lblModel.Text = vehicle.ModelName;
            lblYear.Text = vehicle.Year.ToString();
            lblCurrentMileage.Text = vehicle.CurrentMileage.ToString("N0");
            lblRentalPricePerDay.Text    = vehicle.RentalPricePerDay.ToString("N2");
            lblFuelType.Text   = _GetFuelType(vehicle.FuelTypeID);
            lblVehicleCategory.Text  = _GetVehicleCategory(vehicle.CategoryID);
            lblPlateNumber.Text       = vehicle.PlateNumber;
            lblStatusName.Text       = _GetStatusName(vehicle.StatusID);
            lblVIN.Text     = vehicle.VIN;
            lblColor.Text = clsUiHelper.Deserialize(vehicle.Color).name;
        }
        public void ResetVehicleInfo()
        {
            _vehicle = null;

            lblVehicleID.Text   = "????";
            lblMake.Text  = "????";
            lblModel.Text = "????";
            lblYear.Text  = "????";
            lblCurrentMileage.Text   = "????";
            lblRentalPricePerDay.Text = "????";
            lblFuelType.Text  = "????";
            lblVehicleCategory.Text     = "????";
            lblPlateNumber.Text      = "????";
            lblStatusName.Text      = "????";
            lblVIN.Text    = "????";
            lblColor.Text = "????";
        }
        private string _GetFuelType(enFuelType fuelType)
        {
            return Enum.IsDefined(typeof(enFuelType), fuelType) ? fuelType.ToString() : _Unknown;
        }
        private string _GetVehicleCategory(enVehicleCategory vehicleCategory)
        {
           return Enum.IsDefined(typeof(enVehicleCategory), vehicleCategory) ? vehicleCategory.ToString() : _Unknown;
        }
        private string _GetStatusName(enVehicleStatus status)
        {
            return Enum.IsDefined(typeof(enVehicleStatus),status) ? status.ToString() : _Unknown;
        }
    }
}
