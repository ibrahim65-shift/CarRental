using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.VehicleReturn;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using SharedClass;
using System;
using System.Data;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.VehicleReturn
{
    public class clsVehicleReturnService
    {
        private readonly clsVehicleReturnValidator _validator = new clsVehicleReturnValidator();

        public async Task<clsServiceResult<clsVehicleReturnDto>> GetVehicleReturnByIDAsync(int returnID)
        {
            var entity = await clsVehicleReturnData.GetVehicleReturnByIDAsync(returnID);
            if (entity == null)
                return clsServiceResult<clsVehicleReturnDto>.Fail("لايوجد سيارة مرجعة بهذا المعرف");

            return clsServiceResult<clsVehicleReturnDto>.OK(clsVehicleReturnMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsVehicleReturnDto>> GetVehicleReturnByBookingIDAsync(int bookingID)
        {
            var entity = await clsVehicleReturnData.GetVehicleReturnByBookingIDAsync(bookingID);
            if (entity == null)
                return clsServiceResult<clsVehicleReturnDto>.Fail("لايوجد سيارة مرجعة لهذا الحجز ");

            return clsServiceResult<clsVehicleReturnDto>.OK(clsVehicleReturnMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetPageAsync
            (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var dt = await clsVehicleReturnData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (dt.returnData.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لاتوجد بيانات");

            var paged = new clsPagedResult<DataTable>
            {
                Data = dt.returnData,
                TotalPages = dt.TotalPages
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(paged);
        }
        public  async Task<clsServiceResult<clsVehicleReturnDto>> StartInspectionAsync(int bookingId)
        {
            var validation = await _validator.ValidateStartInspectionAsync(bookingId , DateTime.Now);
            if (!validation.IsValid)
                return clsServiceResult<clsVehicleReturnDto>.Invalid(validation);

            var entity = new clsVehicleReturnEntities
            {
                BookingID = bookingId,
                ActualReturnDate = DateTime.Now,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsVehicleReturnData.StartInspectionAsync(entity);
            if (newID == null)
                return clsServiceResult<clsVehicleReturnDto>.Fail("فشل إرجاع السيارة");

            entity.ReturnID = newID.Value;
            return clsServiceResult<clsVehicleReturnDto>.OK(clsVehicleReturnMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateInspectionAsync(int returnID,clsUpdateInspectionModel model)
        {
            var entity = await clsVehicleReturnData.GetVehicleReturnByIDAsync(returnID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("لايوجد سيارة مرجعة بهذا المعرف");

            var validation =  _validator.ValidateUpdateInspectionAsync(entity.MileageStart, model);
            if(!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            entity.FinalCheckNotes = model.FinalCheckNotes;
            entity.AdditionalCharges = model.AdditionalCharges;
            entity.MileageEnd = model.MileageEnd;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool update = await clsVehicleReturnData.UpdateInspectionAsync(entity);

            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث بيانات السيارة المرجعة");
        }
        public  async Task<clsServiceResult<bool>> FinalizeAsync(int returnID)
        {
            var entity = await clsVehicleReturnData.GetVehicleReturnByIDAsync(returnID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("لايوجد سيارة مرجعة بهذا المعرف");

            var validation = _validator.ValidateFinalizeAsync(entity.MileageStart,entity.MileageEnd);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);


            entity.EditedByUserID = clsCurrentUser.UserID;

            if (entity.ReturnStatusID != enReturnStatus.UnderInspection)
            {
                return clsServiceResult<bool>.Fail("لايمكن إنهاء الإرجاع قبل الفحص");
            }

            var finalize = await clsVehicleReturnData.FinalizeAsync(entity);
            return finalize ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل انهاء بيانات السيارة المرجعة");
        }
        public async Task<clsServiceResult<bool>> MarkAsInvoicedAsync(int returnID)
        {
            var entity = await clsVehicleReturnData.GetVehicleReturnByIDAsync(returnID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("لايوجد سيارة مرجعة بهذا المعرف");

            entity.EditedByUserID = clsCurrentUser.UserID;

            bool invoiced = await clsVehicleReturnData.MarkAsInvoicedAsync(entity);
            return invoiced ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل انهاء الفاتورة");
        }
        public  async Task<clsServiceResult<enReturnStatus?>> GetReturnStatusAsync(int returnID)
        {
            if (!await clsVehicleReturnData.IsVehicleReturnExistsAsync(returnID))
                return clsServiceResult<enReturnStatus?>.Fail("لايوجد سيارة مرجعة");

            return clsServiceResult<enReturnStatus?>.OK(
                await clsVehicleReturnData.GetReturnStatusAsync(returnID));
        }
    }
}
