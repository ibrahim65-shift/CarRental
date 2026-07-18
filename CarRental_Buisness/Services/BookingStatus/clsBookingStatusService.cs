using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.BookingStatus;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Services.BookingStatus
{
    public class clsBookingStatusService
    {
        private readonly clsBookingStatusValidator _validator = new clsBookingStatusValidator();

        public async Task<clsServiceResult<clsBookingStatusDto>> GetBookingStatusByIDAsync(int bookingStatusID)
        {
            var entity = await clsBookingStatusData.GetBookingStatusByIDAsync(bookingStatusID);
            if (entity == null)
                return clsServiceResult<clsBookingStatusDto>.Fail("نوع الحجز غير موجود");

            return clsServiceResult<clsBookingStatusDto>.OK(clsBookingStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsBookingStatusDto>> GetBookingStatusByBookingStatusNameAsync(string statusName)
        {
            var entity = await clsBookingStatusData.GetBookingStatusByStatusNameAsync(statusName);
            if (entity == null)
                return clsServiceResult<clsBookingStatusDto>.Fail("نوع الحجز غير موجود");

            return clsServiceResult<clsBookingStatusDto>.OK(clsBookingStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsBookingStatusDto>>>> GetPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var result = await clsBookingStatusData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (result.bookingStatusData.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsBookingStatusDto>>>.Fail("لاتوجد بيانات");

            var list = result.bookingStatusData.Select(clsBookingStatusMapper.ToDto).ToList();

            var paged = new clsPagedResult<List<clsBookingStatusDto>>
            {
                Data = list,
                TotalPages = result.TotalPages
            };


            return clsServiceResult<clsPagedResult<List<clsBookingStatusDto>>>.OK(paged);
        }
        public async Task<clsServiceResult<clsBookingStatusDto>> AddNewAsync(clsBookingStatusCreateUpdateModel model)
        {
            var validation = await _validator.ValidateCreateUpdateAsync(null,model);
            if (!validation.IsValid)
                return clsServiceResult<clsBookingStatusDto>.Invalid(validation);

            var entity = new clsBookingStatusEntities
            {
                StatusName = model.StatusName,
                Description = model.Description,
            };

            var newID = await clsBookingStatusData.AddNewAsync(entity);
            if (newID == null)
                return clsServiceResult<clsBookingStatusDto>.Fail("فشل إضافة نوع الحجز");

            entity.BookingStatusID = newID.Value;
            return clsServiceResult<clsBookingStatusDto>.OK(clsBookingStatusMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsBookingStatusDto>> UpdateAsync(int BookingStatusID, clsBookingStatusCreateUpdateModel model)
        {
            var entity = await clsBookingStatusData.GetBookingStatusByIDAsync(BookingStatusID);
            if (entity == null)
                return clsServiceResult<clsBookingStatusDto>.Fail("نوع الحجز غير موجود");

            var validation = await _validator.ValidateCreateUpdateAsync(BookingStatusID,model);
            if (!validation.IsValid)
                return clsServiceResult<clsBookingStatusDto>.Invalid(validation);

            entity.StatusName = model.StatusName;
            entity.Description = model.Description;

            bool update = await clsBookingStatusData.UpdateAsync(entity);

            return update ? clsServiceResult<clsBookingStatusDto>.OK(clsBookingStatusMapper.ToDto(entity))
                : clsServiceResult<clsBookingStatusDto>.Fail("فشل تحديث نوع الحجز");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int BookingStatusID)
        {
            if (!await clsBookingStatusData.IsBookingStatusExistsByIDAsync(BookingStatusID))
                return clsServiceResult<bool>.Fail("نوع الحجز غير موجود");


            bool deleted = await clsBookingStatusData.DeleteAsync(BookingStatusID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف نوع الحجز");
        }
    }
}
