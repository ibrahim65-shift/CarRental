using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Customers;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRental.Reports.CustomersReports.Controls
{
    public partial class ctrlCustomerReports : UserControl
    {
        enum enReportType { AllCustomers = 0, TopCustomers = 1, CustomersActivity = 2 }

        private readonly Dictionary<enReportType, Dictionary<string, string>> _columnHeaders =
            new Dictionary<enReportType, Dictionary<string, string>>
            {
                {
                   enReportType.AllCustomers ,
                   new Dictionary<string, string>
                   {
                       {"CustomerID","المعرف"},
                       {"FullName","الاسم الكامل"}, 
                       {"NationalNo","الهوية"},
                       {"Phone","الهاتف"},
                       {"Email","البريد"},
                       {"CreatedDate","تاريخ الإنشاء"},
                       {"TotalBookings","إجمالي الحجوزات"},
                       {"TotalInvoices","إجمالي الفواتير"},
                       {"TotalPayments","إجمالي المدفوعات"},
                       {"TotalRefunds","إجمالي المبالغ المستردة"},
                       {"NetPaid","صافي المدفوعات"}
                   }
                } ,
                 
            
                {
                   enReportType.TopCustomers ,
                   new Dictionary<string, string>
                   {
                       {"CustomerID","المعرف"},
                       {"FullName","الاسم الكامل"}, 
                       {"NationalNo","الهوية"},
                       {"Phone","الهاتف"},
                       {"Email","البريد"},
                       {"TotalBookings","إجمالي الحجوزات"},
                       {"TotalInvoices","إجمالي الفواتير"},
                       {"TotalPayments","إجمالي المدفوعات"},
                       {"TotalRefunds","إجمالي المبالغ المستردة"},
                       {"NetPaid","صافي المدفوعات"}
                   }
                } ,
                 
            
                {
                   enReportType.CustomersActivity ,
                   new Dictionary<string, string>
                   {
                       {"CustomerID","المعرف"},
                       {"FullName","الاسم الكامل"}, 
                       {"NationalNo","الهوية"},
                       {"Phone","الهاتف"},
                       {"Email","البريد"},
                       {"TotalBookings","إجمالي الحجوزات"},
                       {"FirstBookingDate","أول حجز"},
                       {"LastBookingDate","اخر حجز"},
                       {"DaysSinceLastBooking","عدد الأيام منذ اخر حجز"},
                       {"CustomerStatus","حالة العميل"}
                   }
                } 
                 
            
            };

        private readonly Dictionary<enReportType, string> _titles = new Dictionary<enReportType, string>
        {
            {enReportType.AllCustomers , "تقرير جميع العملاء"}, 
            {enReportType.TopCustomers , "تقرير أفضل العملاء"}, 
            {enReportType.CustomersActivity , "تقرير نشاط العملاء"}
        };

        private readonly clsCustomerService _customerService;
        private readonly frmMain _frmMain;

        private bool _isLoading = false;
        enReportType _reportType = enReportType.AllCustomers;
        public ctrlCustomerReports(frmMain main)
        {
            InitializeComponent();
            _customerService = new clsCustomerService();
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

            bool useDateFilter = _reportType == enReportType.AllCustomers;
            bool useTop = _reportType == enReportType.TopCustomers;

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

            if(_reportType == enReportType.AllCustomers && dtpFrom.Value.Date > dtpTo.Value.Date)
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
                    case enReportType.AllCustomers:
                        await _LoadReportAsync(() =>
                            _customerService.GetReportAllCustomersAsync(dtpFrom.Value.Date, dtpTo.Value.Date));
                        break;

                    case enReportType.TopCustomers:
                        await _LoadReportAsync(()=> _customerService.GetReportTopCustomersAsync((int)numericUpDownTop.Value));
                        break;

                    case enReportType.CustomersActivity:
                        await _LoadReportAsync(() => _customerService.GetReportCustomerActivityAsync());
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
                clsEventLogger.LogException("ctrlCustomerReports._LoadReportAsync", ex);
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