using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Vehicles;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Reports.VehiclesReports.Controls
{
    public partial class ctrlVehiclesReports : UserControl
    {
        enum enReportType { AllVehicles = 0, MostRentedVehicles = 1, VehicleMaintenance = 2 }

        private readonly Dictionary<enReportType, Dictionary<string, string>> _columnHeaders =
            new Dictionary<enReportType, Dictionary<string, string>>
            {
                {
                   enReportType.AllVehicles ,
                   new Dictionary<string, string>
                   {
                       {"VehicleID","المعرف"},
                       {"Make","الماركة"}, 
                       {"Model","الموديل"},
                       {"Year","السنة"},
                       {"PlateNumber","اللوحة"},
                       {"Category","الفئة"},
                       {"FuelType","النوع"},
                       {"RentalPricePerDay","سعر الإيجار اليومي"},
                       {"Status","الحالة"},
                       {"CurrentMileage","المسافة المقطوعة الحالية"},
                       {"TotalBookings","إجمالي الحجوزات"},
                       {"TotalRevenue","الإيرادات"},
                       {"TotalMaintenanceCost","إجمالي تكلفة الصيانة"}
                   }
                } ,
                 
            
                {
                   enReportType.MostRentedVehicles ,
                   new Dictionary<string, string>
                   {
                       {"VehicleID","المعرف"},
                       {"Make","الماركة"},
                       {"Model","الموديل"},
                       {"Year","السنة"},
                       {"PlateNumber","اللوحة"},
                       {"Category","الفئة"},
                       {"FuelType","النوع"},
                       {"RentalPricePerDay","سعر الإيجار اليومي"},
                       {"Status","الحالة"},
                       {"CurrentMileage","المسافة المقطوعة الحالية"},
                       {"TotalBookings","إجمالي الحجوزات"},
                       {"TotalRentalDays","إجمالي أيام الإيجار"},
                       {"TotalRevenue","الإيرادات"}
                   }
                } ,
                 
            
                {
                   enReportType.VehicleMaintenance ,
                   new Dictionary<string, string>
                   {
                       {"VehicleID","المعرف"},
                       {"Make","الماركة"},
                       {"Model","الموديل"},
                       {"Year","السنة"},
                       {"PlateNumber","اللوحة"},
                       {"Category","الفئة"},
                       {"FuelType","النوع"},
                       {"RentalPricePerDay","سعر الإيجار اليومي"},
                       {"Status","الحالة"},
                       {"CurrentMileage","المسافة المقطوعة الحالية"},
                       {"NumberOfMaintenances","عدد الصيانات"},
                       {"TotalMaintenancesCost","إجمالي تكلفة الصيانة"},
                       {"LastMaintenanceDate","اخر صيانة"},
                       {"AverageMaintenanceCost","متوسط تكلفة الصيانة"},
                   }
                } 
                 
            
            };

        private readonly Dictionary<enReportType, string> _titles = new Dictionary<enReportType, string>
        {
            {enReportType.AllVehicles , "تقرير جميع المركبات"}, 
            {enReportType.MostRentedVehicles , "تقرير المركبات الأكثر تأجيراً"}, 
            {enReportType.VehicleMaintenance , "تقرير صيانة المركبات"}
        };

        private readonly clsVehicleService _VehicleService;
        private readonly frmMain _frmMain;

        private bool _isLoading = false;
        enReportType _reportType = enReportType.AllVehicles;
        public ctrlVehiclesReports(frmMain main)
        {
            InitializeComponent();
            _VehicleService = new clsVehicleService();
            _frmMain = main;
            cbReportType.SelectedIndex = 0;
        }

        private async void btnGenerateReport_Click(object sender, EventArgs e)
        {
            await _GenerateReportAsync();
        }
        private void cbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvReportViewer.DataSource = null;
            _ShowEmptyDataState();


            _reportType = (enReportType)cbReportType.SelectedIndex;

            bool useDateFilter = _reportType == enReportType.MostRentedVehicles || _reportType == enReportType.VehicleMaintenance;
            bool useTop = _reportType == enReportType.MostRentedVehicles;

            labelTop.Visible = useTop;
            numericUpDownTop.Visible = useTop;

            labelFromDate.Visible = useDateFilter;
            dtpFrom.Visible = useDateFilter;
            labelToDate.Visible = useDateFilter;
            dtpTo.Visible = useDateFilter;
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            _ExportToExcel();
        }

        // ================== METHODS ==================

        private async Task _GenerateReportAsync()
        {
            if (_isLoading)
                return;

            if( (_reportType == enReportType.MostRentedVehicles || _reportType == enReportType.VehicleMaintenance)
                && dtpFrom.Value.Date > dtpTo.Value.Date)
            {
                clsMessages.ShowError("تاريخ البداية يجب أن يكون قبل تاريخ النهاية");
                return;
            }

            try
            {
                _isLoading = true;
                btnGenerateReport.Enabled = false;

                switch (_reportType)
                {
                    case enReportType.AllVehicles:
                        await _LoadReportAsync(() =>
                            _VehicleService.GetReportAllVehiclesAsync());
                        break;

                    case enReportType.MostRentedVehicles:
                        await _LoadReportAsync(()=> _VehicleService.
                                    GetReportMostRentedVehiclesAsync((int)numericUpDownTop.Value,dtpFrom.Value.Date,dtpTo.Value.Date));
                        break;

                    case enReportType.VehicleMaintenance:
                        await _LoadReportAsync(() => _VehicleService
                                                      .GetReportVehicleMaintenanceAsync(dtpFrom.Value.Date, dtpTo.Value.Date));
                        break;

                    default:
                        throw new InvalidOperationException("نوع التقرير غير معروف");
                }
            }
            finally
            {
                _isLoading = false;
                btnGenerateReport.Enabled = true;
            }
        }
        private async Task _LoadReportAsync(Func<Task<clsServiceResult<DataTable>>> loader)
        {
            try
            {
                var result = await loader();
                if (!result.Success)
                {
                    dgvReportViewer.DataSource = null;
                    clsMessages.ShowError(result.ErrorMessage);
                    _ShowEmptyDataState();
                    return;
                }
                dgvReportViewer.DataSource = null;
                dgvReportViewer.DataSource = result.Data;
                _InitializeColumns();
                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlVehiclesReports._LoadReportAsync", ex);
                _ShowServerErrorState();
            }
        }
        private void _InitializeColumns()
        {
            if (dgvReportViewer.DataSource == null || dgvReportViewer.Rows.Count == 0)
                return;

            if (!_columnHeaders.TryGetValue(_reportType, out var headers))
                return;

            foreach(var item in headers)
            {
                _SetColumnHeader(item.Key, item.Value);
            }
        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvReportViewer.Columns.Contains(columnName))
                dgvReportViewer.Columns[columnName].HeaderText = headerText;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvReportViewer.Rows.Count == 0;
            pnlState.Visible = isEmpty;

            if (!isEmpty)
                return;

            lblTitleState.Text = Properties.Resources.EmptyDataStateTitle ;
            lblDescriptionState.Text =  Properties.Resources.EmptyReportDescription;
            
        }
        private void _ShowServerErrorState()
        {
            pnlState.Visible = true;
            lblTitleState.Text = Properties.Resources.ServerErrorTitle;
            lblDescriptionState.Text = Properties.Resources.ServerErrorDescription;
        }
        private void _ExportToExcel()
        {
            var data = dgvReportViewer.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var exportData = _CreateExportTable(data);
            clsExcelHelper.Export(_frmMain, exportData, _titles[_reportType]);
        }
        private DataTable _CreateExportTable(DataTable source)
        {
            var exportTable = new DataTable();

            var mapping = _columnHeaders[_reportType];

            foreach (var column in mapping)
            {
                exportTable.Columns.Add(column.Value);
            }

            exportTable.BeginLoadData();

            foreach(DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                foreach(var item in mapping)
                {
                    var value = row[item.Key];

                    if(value is DateTime dt)
                        value = clsUtil.FormatDate(dt);

                    newRow[item.Value] = value;
                }

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();

            return exportTable;
        }
    }
}