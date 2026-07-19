using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Models.Maintenance;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Invoices;
using CarRental_Buisness.Services.Maintenance;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CarRental_Buisness.Services.WorkFlow
{
    public class clsMaintenanceWorkflowService
    {
        private readonly clsMaintenanceService _maintenanceService;
        private readonly clsInvoicesService _invoiceService;

        public clsMaintenanceWorkflowService(clsMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService;
            _invoiceService  = new clsInvoicesService();
        }

        public async Task<clsServiceResult<clsMaintenanceDto>> CreateMaintenanceAsync(clsMaintenanceAddNewModel maintenanceModel , clsApplicationSettings settings)
        {
            using(var scope = new TransactionScope(TransactionScopeOption.Required , 
                                                    new TransactionOptions
                                                    {
                                                        IsolationLevel = IsolationLevel.ReadCommitted,
                                                    },TransactionScopeAsyncFlowOption.Enabled))
            {

                var maintenanceResult = await _maintenanceService.AddNewAsync(maintenanceModel);
                if (!maintenanceResult.Success)
                    return maintenanceResult;

                var invoiceModel = _BuildMaintenanceInvoice(maintenanceResult.Data , settings);

                var invoiceResult = await _invoiceService.AddNewAsync(invoiceModel);
                if (!invoiceResult.Success)
                {
                    if (!string.IsNullOrWhiteSpace(invoiceResult.ErrorMessage))
                        return clsServiceResult<clsMaintenanceDto>.Fail(invoiceResult.ErrorMessage);
                    else
                        return clsServiceResult<clsMaintenanceDto>.Invalid(invoiceResult.Validation);
                }

                scope.Complete();
                return maintenanceResult;
            }
        }
        public async Task<clsServiceResult<bool>> UpdateMaintenanceAsync(int maintenanceId,clsMaintenanceUpdateModel model
            , clsApplicationSettings settings)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required , 
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },TransactionScopeAsyncFlowOption.Enabled))
            {

                var maintenanceResult = await _maintenanceService.UpdateAsync(maintenanceId, model);
                if (!maintenanceResult.Success)
                    return maintenanceResult;

                var maintenance = await _maintenanceService.GetMaintenanceByMaintenanceIDAsync(maintenanceId);
                if(!maintenance.Success)
                    return clsServiceResult<bool>.Fail(maintenance.ErrorMessage);

                var invoiceModel = _BuildUpdateMaintenanceInvoice(maintenance.Data , settings);

                var invoiceResult = await _invoiceService.UpdateLinkedInvoiceAsync(maintenanceId , invoiceModel);
                if (!invoiceResult.Success)
                {
                    if (!string.IsNullOrWhiteSpace(invoiceResult.ErrorMessage))
                        return clsServiceResult<bool>.Fail(invoiceResult.ErrorMessage);
                    else
                        return clsServiceResult<bool>.Invalid(invoiceResult.Validation);
                }

                scope.Complete();
                return clsServiceResult<bool>.OK(true);
            }
        }
        private clsInvoicesAddNewModel _BuildMaintenanceInvoice(clsMaintenanceDto maintenance , clsApplicationSettings settings)
        {
            return new clsInvoicesAddNewModel
            {
                InvoiceTypeID = enInvoiceTypes.Maintenance,
                MaintenanceID = maintenance.MaintenanceID,
                BaseAmount = maintenance.Cost,
                AdditionalCharges = 0m,
                LateFees = 0m,
                TaxAmount = 0m,
                DiscountAmount = 0m,
                CurrencyCode = settings.CurrencyCode,
                Notes = $"صيانة المركبة رقم {maintenance.VehicleID}"
            };
        }
        private clsUpdateLinkedInvoiceModel _BuildUpdateMaintenanceInvoice(clsMaintenanceDto maintenance , clsApplicationSettings settings)
        {
            return new clsUpdateLinkedInvoiceModel
            {
                EntityID = maintenance.MaintenanceID,
                InvoiceType = enInvoiceTypes.Maintenance,
                BaseAmount = maintenance.Cost,
                AdditionalCharges = 0m,
                LateFees = 0m,
                TaxAmount =0m,
                DiscountAmount = 0m,
                CurrencyCode = settings.CurrencyCode,
                Notes = "تم تحديث الفاتورة بعد تعديل بيانات الصيانة"
            };
        }
    }
}
