using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Models.VehicleDamage;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Invoices;
using CarRental_Buisness.Services.VehicleDamage;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CarRental_Buisness.Services.WorkFlow
{
    public class clsVehicleDamageWorkflowService
    {
        private readonly clsVehicleDamageService _VehicleDamageService;
        private readonly clsInvoicesService _invoiceService;

        public clsVehicleDamageWorkflowService(clsVehicleDamageService VehicleDamageService)
        {
            _VehicleDamageService = VehicleDamageService;
            _invoiceService = new clsInvoicesService();
        }

        public async Task<clsServiceResult<clsVehicleDamageDto>> CreateVehicleDamageAsync(clsVehicleDamageAddNewModel VehicleDamageModel, clsApplicationSettings settings)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                                    new TransactionOptions
                                                    {
                                                        IsolationLevel = IsolationLevel.ReadCommitted,
                                                    }, TransactionScopeAsyncFlowOption.Enabled))
            {

                var VehicleDamageResult = await _VehicleDamageService.AddNewAsync(VehicleDamageModel);
                if (!VehicleDamageResult.Success)
                    return VehicleDamageResult;

                var invoiceModel = _BuildVehicleDamageInvoice(VehicleDamageResult.Data, settings);

                var invoiceResult = await _invoiceService.AddNewAsync(invoiceModel);
                if (!invoiceResult.Success)
                {
                    if (!string.IsNullOrWhiteSpace(invoiceResult.ErrorMessage))
                        return clsServiceResult<clsVehicleDamageDto>.Fail(invoiceResult.ErrorMessage);
                    else
                        return clsServiceResult<clsVehicleDamageDto>.Invalid(invoiceResult.Validation);
                }

                scope.Complete();
                return VehicleDamageResult;
            }
        }
        public async Task<clsServiceResult<bool>> UpdateVehicleDamageAsync(int damageId, clsVehicleDamageUpdateModel model
            , clsApplicationSettings settings)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                }, TransactionScopeAsyncFlowOption.Enabled))
            {

                var VehicleDamageResult = await _VehicleDamageService.UpdateAsync(damageId, model);
                if (!VehicleDamageResult.Success)
                    return VehicleDamageResult;

                var VehicleDamage = await _VehicleDamageService.GetVehicleDamageByIDAsync(damageId);
                if (!VehicleDamage.Success)
                    return clsServiceResult<bool>.Fail(VehicleDamage.ErrorMessage);

                var invoiceModel = _BuildUpdateVehicleDamageInvoice(VehicleDamage.Data, settings);

                var invoiceResult = await _invoiceService.UpdateLinkedInvoiceAsync(damageId, invoiceModel);
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
        private clsInvoicesAddNewModel _BuildVehicleDamageInvoice(clsVehicleDamageDto damage, clsApplicationSettings settings)
        {
            return new clsInvoicesAddNewModel
            {
                InvoiceTypeID = enInvoiceTypes.VehicleDamage,
                DamageID = damage.DamageID,
                BaseAmount = damage.EstimatedCost.Value,
                AdditionalCharges = 0m,
                LateFees = 0m,
                TaxAmount = damage.EstimatedCost.Value * settings.TaxRate,
                DiscountAmount = 0m,
                CurrencyCode = settings.CurrencyCode,
                Notes = $"ضرر للمركبة رقم {damage.VehicleID}"
            };
        }
        private clsUpdateLinkedInvoiceModel _BuildUpdateVehicleDamageInvoice(clsVehicleDamageDto damage, clsApplicationSettings settings)
        {
            return new clsUpdateLinkedInvoiceModel
            {
                EntityID = damage.DamageID,
                InvoiceType = enInvoiceTypes.VehicleDamage,
                BaseAmount = damage.EstimatedCost.Value,
                AdditionalCharges = 0m,
                LateFees = 0m,
                TaxAmount = damage.EstimatedCost.Value * settings.TaxRate,
                DiscountAmount = 0m,
                CurrencyCode = settings.CurrencyCode,
                Notes = "تم تحديث الفاتورة بعد تعديل بيانات الضرر"
            };
        }
    }
}
