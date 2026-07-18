using CarRental.Helper;
using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.FuelTypes;
using CarRental_Buisness.Services.Customers;
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

namespace CarRental.Reports.CustomersReports.Controls
{
    public partial class ctrlCustomerReports : UserControl
    {
        public static class Columns
        {
            public const string CustomerID = nameof(CustomerID);
            public const string FullName     = nameof(FullName);
            public const string NationalNo   = nameof(NationalNo);
            public const string Phone        = nameof(Phone);
            public const string Email        = nameof(Email);
            public const string CreatedDate  = nameof(CreatedDate);
            public const string TotalBookings= nameof(TotalBookings);
            public const string TotalInvoices= nameof(TotalInvoices);
            public const string TotalPayments= nameof(TotalPayments);
            public const string TotalRefunds = nameof(TotalRefunds);
            public const string NetPaid = nameof(NetPaid);
        }
        enum enReportType { allCustomers = 0, topCustomers = 1, outstandingCustomers = 2 }

        private readonly clsCustomerService _customerService;
        private readonly frmMain _frmMain;

        enReportType _reportType = enReportType.allCustomers;
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
            _reportType = (enReportType)cbReportType.SelectedIndex;
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            _ExportToExcel();
        }

        // ================== METHODES ==================

        private async Task _GenerateReportAsync()
        {
            switch(_reportType)
            {
                case enReportType.allCustomers:
                    await _GetAllCustomersAsync();
                    break;

                case enReportType.topCustomers:
                    break;

                case enReportType.outstandingCustomers:
    
                    break;


            }
        }
        private async Task _GetAllCustomersAsync()
        {
            try
            {
                var result = await _customerService.GetReportAllCustomersAsync(dtpFrom.Value.Date, dtpTo.Value.Date);
                if (!result.Success)
                {
                    clsMessages.ShowError(result.ErrorMessage);
                }

                dgvReportViewer.DataSource = null;
                dgvReportViewer.DataSource = result.Data;
                _InitializeColumns();
                _ShowEmptyDataState();
            }
            catch (Exception ex)
            {
                clsEventLogger.LogException("ctrlCustomerReports._GetAllCustomersAsync", ex);
                _ShowServerErrorState();
            }
        }
        private void _InitializeColumns()
        {
            if (dgvReportViewer.DataSource == null || dgvReportViewer.Rows.Count == 0)
                return;

            _SetColumnHeader(Columns.CustomerID,"المعرف"                 );
            _SetColumnHeader(Columns.FullName,"الاسم الكامل"            );
            _SetColumnHeader(Columns.NationalNo,"الهوية"                 );
            _SetColumnHeader(Columns.Phone,"الهاتف"                 );
            _SetColumnHeader(Columns.Email,"البريد"                 );
            _SetColumnHeader(Columns.CreatedDate,"تاريخ الإنشاء"           );
            _SetColumnHeader(Columns.TotalBookings,"إجمالي الحجوزات"        );
            _SetColumnHeader(Columns.TotalInvoices,"إجمالي الفواتير"        );
            _SetColumnHeader(Columns.TotalPayments,"إجمالي المدفوعات"       );
            _SetColumnHeader(Columns.TotalRefunds,"إجمالي المبالغ المستردة");
            _SetColumnHeader(Columns.NetPaid,"صافي المدفوعات");

        }
        private void _SetColumnHeader(string columnName, string headerText)
        {
            if (dgvReportViewer.Columns.Contains(columnName))
                dgvReportViewer.Columns[columnName].HeaderText = headerText;
        }
        private void _ShowEmptyDataState()
        {
            bool isEmpty = dgvReportViewer.Rows.Count == 0;
            lblTitleState.Text = isEmpty ? Properties.Resources.EmptyDataStateTitle : "";
            lblDescriptionState.Text = isEmpty ? Properties.Resources.EmptyReportDescription : "";
            pnlState.Visible = isEmpty;
        }
        private void _ShowServerErrorState()
        {
            lblTitleState.Text = Properties.Resources.ServerErrorTitle;
            lblDescriptionState.Text = Properties.Resources.ServerErrorDescription;
            pnlState.Visible = true;
        }
        private void _ExportToExcel()
        {
            var data = dgvReportViewer.DataSource as DataTable;

            if (data == null || data.Rows.Count == 0)
            {
                clsMessages.ShowError("لا توجد بيانات للتصدير.");
                return;
            }

            var optimizedData = _CreateOptimizedExportData(data);
            clsExcelHelper.Export(_frmMain, optimizedData, "تقرير العملاء");
        }
        private DataTable _CreateOptimizedExportData(DataTable source)
        {
            var exportTable = new DataTable();

            exportTable.Columns.Add("المعرف"                 , typeof(int));
            exportTable.Columns.Add("الاسم الكامل"            , typeof(string));
            exportTable.Columns.Add("الهوية"                 , typeof(string));
            exportTable.Columns.Add("الهاتف"                 , typeof(string));
            exportTable.Columns.Add("البريد"                 , typeof(string));
            exportTable.Columns.Add("تاريخ الإنشاء"           , typeof(string));
            exportTable.Columns.Add("إجمالي الحجوزات"        , typeof(int));
            exportTable.Columns.Add("إجمالي الفواتير"        , typeof(decimal));
            exportTable.Columns.Add("إجمالي المدفوعات"       , typeof(decimal));
            exportTable.Columns.Add("إجمالي المبالغ المستردة", typeof(decimal));
            exportTable.Columns.Add("صافي المدفوعات"         , typeof(decimal));

            exportTable.BeginLoadData();

            foreach (DataRow row in source.Rows)
            {
                var newRow = exportTable.NewRow();

                newRow["المعرف"]     = row[Columns.CustomerID];
                newRow["الاسم الكامل"] = row[Columns.FullName];
                newRow["الهوية"] = row[Columns.NationalNo];
                newRow["الهاتف"] = row[Columns.Phone];
                newRow["البريد"] = row[Columns.Email];
                newRow["تاريخ الإنشاء"] = clsUtil.FormatDate(row[Columns.CreatedDate]);
                newRow["إجمالي الحجوزات"] = row[Columns.TotalBookings];
                newRow["إجمالي الفواتير"] = row[Columns.TotalInvoices];
                newRow["إجمالي المدفوعات"] = row[Columns.TotalPayments];
                newRow["إجمالي المبالغ المستردة"] = row[Columns.TotalRefunds];
                newRow["صافي المدفوعات"] = row[Columns.NetPaid];

                exportTable.Rows.Add(newRow);
            }

            exportTable.EndLoadData();
            return exportTable;
        }
    }
}
