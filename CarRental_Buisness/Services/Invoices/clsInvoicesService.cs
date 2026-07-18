using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.FuelTypes;
using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Invoices;
using CarRental_DataAccess;
using CarRental_Entities;
using CarRental_Entities.Invoices;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CarRental_Buisness.Services.Invoices
{
    public class clsInvoicesService
    {
        private readonly clsInvoicesValidator _validator = new clsInvoicesValidator();

        public async Task<clsServiceResult<clsInvoicesDto>> GetInvoiceByIDAsync(int InvoiceID)
        {
            var entity = await clsInvoicesData.GetInvoiceByIDAsync(InvoiceID);
            if (entity == null)
                return clsServiceResult<clsInvoicesDto>.Fail("الفاتورة غير موجودة");

            return clsServiceResult<clsInvoicesDto>.OK(clsInvoicesMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsInvoicesDto>> GetInvoiceByBookingIDAsync(int BookingID)
        {
            var entity = await clsInvoicesData.GetInvoiceByBookingIDAsync(BookingID);
            if (entity == null)
                return clsServiceResult<clsInvoicesDto>.Fail("الفاتورة غير موجودة");

            return clsServiceResult<clsInvoicesDto>.OK(clsInvoicesMapper.ToDto(entity));
        }

        public async Task<clsServiceResult<clsPagedResult<List<clsInvoicesViewDto>>>> GetPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsInvoicesData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.InvoicesData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsInvoicesViewDto>>>.Fail("لاتوجد بيانات");

            var list = result.InvoicesData.Select(clsInvoicesViewMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsInvoicesViewDto>>
            {
                Data = list,
                TotalPages = result.TotalPages
            };

            return clsServiceResult<clsPagedResult<List<clsInvoicesViewDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsInvoicesDto>> AddNewAsync(clsInvoicesAddNewModel model)
        {
            var validation = await _validator.ValidateAddNewAsync(model);
            if (!validation.IsValid)
                return clsServiceResult<clsInvoicesDto>.Invalid(validation);

            var entity = new clsInvoiceEntities
            {
                InvoiceTypeID = model.InvoiceTypeID,
                BookingID = model.BookingID,
                MaintenanceID = model.MaintenanceID,
                DamageID = model.DamageID,
                BaseAmount = model.BaseAmount,
                AdditionalCharges = model.AdditionalCharges,
                LateFees = model.LateFees,
                TaxAmount = model.TaxAmount,
                DiscountAmount = model.DiscountAmount,
                CurrencyCode = model.CurrencyCode.Trim().ToUpper(),
                Notes = model.Notes,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsInvoicesData.AddNewAsync(entity);

            if (newID == null)
                return clsServiceResult<clsInvoicesDto>.Fail("فشل إضافة الفاتورة");

            var addEntity = await clsInvoicesData.GetInvoiceByIDAsync(newID.Value);

            return clsServiceResult<clsInvoicesDto>.OK(clsInvoicesMapper.ToDto(entity));    
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int InvoiceID, clsInvoicesUpdateModel model)
        {
            var entity = await clsInvoicesData.GetInvoiceByIDAsync(InvoiceID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("الفاتورة غير موجودة");

            var validation = await _validator.ValidateUpdateAsync(InvoiceID, model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);


            entity.AdditionalCharges = model.AdditionalCharges;
            entity.LateFees = model.LateFees;
            entity.DiscountAmount = model.DiscountAmount;
            entity.Notes = model.Notes;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool update = await clsInvoicesData.UpdateAsync(entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث الفاتورة");
        }
        public async Task<clsServiceResult<bool>> UpdateLinkedInvoiceAsync(int entityId, clsUpdateLinkedInvoiceModel model)
        {
            var entity = await GetLinkedInvoiceAsync(entityId, model.InvoiceType);
            if (entity == null)
                return clsServiceResult<bool>.Fail("الفاتورة غير موجودة");

            var validation = await _validator.ValidateUpdateLinkedInvoiceAsync(entity.InvoiceID, model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            entity.InvoiceTypeID = model.InvoiceType;
            entity.BaseAmount = model.BaseAmount;
            entity.AdditionalCharges = model.AdditionalCharges;
            entity.LateFees = model.LateFees;
            entity.DiscountAmount = model.DiscountAmount;
            entity.TaxAmount = model.TaxAmount;
            entity.CurrencyCode = model.CurrencyCode;
            entity.Notes = model.Notes;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool update = await clsInvoicesData.UpdateLinkedInvoiceAsync(entityId,entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث الفاتورة");
        }
        private async Task<clsInvoiceEntities> GetLinkedInvoiceAsync(int entityId , enInvoiceTypes invoiceType)
        {
            
            switch (invoiceType)
            {
                case enInvoiceTypes.Booking:
                   return await clsInvoicesData.GetInvoiceByBookingIDAsync(entityId);

                case enInvoiceTypes.Maintenance:
                    return await clsInvoicesData.GetInvoiceByMaintenanceIDAsync(entityId);

                case enInvoiceTypes.VehicleDamage:
                    return await clsInvoicesData.GetInvoiceByDamageIDAsync(entityId);

                default: 
                    return null;
            }
        }


    }
}
