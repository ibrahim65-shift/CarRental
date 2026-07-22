using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Locations;
using CarRental_Buisness.Models.RentalBooking;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;


namespace CarRental_Buisness.Services.RentalBooking
{
    public class clsRentalBookingService
    {
        private readonly clsRentalBookingValidator _validator = new clsRentalBookingValidator();

        public async Task<clsServiceResult<clsRentalBookingDto>> GetRentalBookingByIDAsync(int BookingID)
        {
            var entity  = await clsRentalBookingData.GetRentalBookingByIDAsync(BookingID);
            if (entity == null)
                return clsServiceResult<clsRentalBookingDto>.Fail("الحجز غير موجود");

            return clsServiceResult<clsRentalBookingDto>.OK(clsRentalBookingMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsRentalBookingDto>>>> GetRentalBookingByCustomerIDAsync(int CustomerID)
        {
            var entities = await clsRentalBookingData.GetRentalBookingByCustomerIDAsync(CustomerID);
            if (entities == null || entities.Count==0)
                return clsServiceResult<clsPagedResult<List<clsRentalBookingDto>>>.Fail("لاتوجد حجوزات لهذا العميل");

            var list = new clsPagedResult<List<clsRentalBookingDto>>
            {
                Data = entities.Select(clsRentalBookingMapper.ToDto).ToList()
            };

            return clsServiceResult<clsPagedResult<List<clsRentalBookingDto>>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<List<clsRentalBookingDto>>>> GetRentalBookingByVehicleIDAsync(int VehicleID)
        {
            var entities = await clsRentalBookingData.GetRentalBookingByVehicleIDAsync(VehicleID);
            if (entities == null || entities.Count == 0)
                return clsServiceResult<clsPagedResult<List<clsRentalBookingDto>>>.Fail("لاتوجد حجوزات لهذه المركبة");

            var list = new clsPagedResult<List<clsRentalBookingDto>>
            {
                Data = entities.Select(clsRentalBookingMapper.ToDto).ToList()
            };

            return clsServiceResult<clsPagedResult<List<clsRentalBookingDto>>>.OK(list);
        }
        public async Task<clsServiceResult<clsPagedResult<DataTable>>> GetPageAsync
           (int PageNumber, int PageSize, string FilterColumn = null, string FilterValue = null)
        {
            var dt = await clsRentalBookingData.GetPageAsync(PageNumber, PageSize, FilterColumn, FilterValue);
            if (dt.rentalBookingData.Rows.Count == 0)
                return clsServiceResult<clsPagedResult<DataTable>>.Fail("لاتوجد بيانات");

            var paged = new clsPagedResult<DataTable>
            {
                Data = dt.rentalBookingData,
                TotalPages = dt.TotalPage
            };

            return clsServiceResult<clsPagedResult<DataTable>>.OK(paged);
        }
        public async Task<clsServiceResult<clsRentalBookingDto>> AddNewAsync(clsRentalBookingAddNewModel model , DateTime driverLicenseExpiry)
        {
            var validation = await _validator.ValidateAddNewAsync(model ,driverLicenseExpiry);
            if (!validation.IsValid)
                return clsServiceResult<clsRentalBookingDto>.Invalid(validation);

            int rentalDays = (model.RentalEndDate.Date - model.RentalStartDate.Date).Days;
            decimal? rentalPriceDay = 
                await clsRatePlansData.GetRentalPricePerDayAsync(model.VehicleID, model.RentalStartDate, rentalDays);

            if(!rentalPriceDay.HasValue)
            {
                return clsServiceResult<clsRentalBookingDto>.Fail("تعذر احتساب سعر الإيجار");
            }

            var entity = new clsRentalBookingEntities
            {
                CustomerID = model.CustomerID,
                VehicleID = model.VehicleID,
                RentalPricePerDay = rentalPriceDay.Value, 
                RentalStartDate = model.RentalStartDate,
                RentalEndDate = model.RentalEndDate,
                PickupLocationID = model.PickupLocationID,
                DropOffLocationID = model.DropOffLocationID,
                InitialCheckNotes = model.InitialCheckNotes,
                CreatedByUserID = clsCurrentUser.UserID
            };

            var newID = await clsRentalBookingData.AddNewAsync(entity);

            if (newID == null)
                return clsServiceResult<clsRentalBookingDto>.Fail("فشل إضافة الحجز");

            entity.BookingID = newID.Value;
            return clsServiceResult<clsRentalBookingDto>.OK(clsRentalBookingMapper.ToDto(entity));
        }
        public async Task<clsServiceResult<bool>> UpdateAsync(int BookingID , clsRentalBookingUpdateModel model)
        {
            var entity = await clsRentalBookingData.GetRentalBookingByIDAsync(BookingID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("الحجز غير موجود");

            var validation = await _validator.ValidateUpdateAsync(BookingID,model);
            if (!validation.IsValid)
                return clsServiceResult<bool>.Invalid(validation);

            if (entity.BookingStatusID != enBookingStatus.Draft)
                return clsServiceResult<bool>.Fail("لايمكن تحديث البيانات في هذه الحالة");

            int rentalDays = (model.RentalEndDate.Date - model.RentalStartDate.Date).Days;
            decimal? rentalPriceDay =
                await clsRatePlansData.GetRentalPricePerDayAsync(model.VehicleID, model.RentalStartDate, rentalDays);

            if (!rentalPriceDay.HasValue)
            {
                return clsServiceResult<bool>.Fail("تعذر احتساب سعر الإيجار");
            }

            entity.VehicleID = model.VehicleID;
            entity.RentalPricePerDay = rentalPriceDay.Value;
            entity.RentalStartDate = model.RentalStartDate;
            entity.RentalEndDate = model.RentalEndDate;
            entity.PickupLocationID = model.PickupLocationID;
            entity.DropOffLocationID = model.DropOffLocationID;
            entity.InitialCheckNotes = model.InitialCheckNotes;
            entity.EditedByUserID = clsCurrentUser.UserID;

            bool update = await clsRentalBookingData.UpdateAsync(entity);
            return update ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث الحجز");
        }
        public async Task<clsServiceResult<bool>> DeleteAsync(int BookingID)
        {
            if(!await clsRentalBookingData.IsRentalBookingExistsAsync(BookingID))
            {
                return clsServiceResult<bool>.Fail("معرف الحجز غير صالح");
            }

            bool deleted = await clsRentalBookingData.DeleteAsync(BookingID);
            return deleted ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل حذف الحجز");
        }
        public async Task<clsServiceResult<bool>> UpdateStatusAsync(int BookingID , enBookingStatus newStatus)
        {
            var entity = await clsRentalBookingData.GetRentalBookingByIDAsync(BookingID);
            if (entity == null)
                return clsServiceResult<bool>.Fail("الحجز غير موجود");

            if (!IsValidStatusTransition(entity.BookingStatusID, newStatus))
                return clsServiceResult<bool>.Fail("لايمكن الانتقال الى هذه الحالة");

            bool updated = await clsRentalBookingData.UpdateStatusAsync(BookingID, (int)newStatus,
                clsCurrentUser.UserID);

            return updated ? clsServiceResult<bool>.OK(true) : clsServiceResult<bool>.Fail("فشل تحديث الحالة");
        }
        private bool IsValidStatusTransition(enBookingStatus current , enBookingStatus next)
        {
            switch(current)
            {
                case enBookingStatus.Draft:
                    return next==enBookingStatus.Confirmed || next ==enBookingStatus.Cancelled;

                case enBookingStatus.Active:
                    return  next==enBookingStatus.Completed;

                case enBookingStatus.Confirmed:
                    return next == enBookingStatus.Active ||
                        next == enBookingStatus.Cancelled ||
                        next == enBookingStatus.NoShow;

            }

            return false;
        }
        public List<enBookingStatus> GetAllowedStatuses(enBookingStatus current)
        {
            switch(current)
            {
                case enBookingStatus.Draft:
                    return new List<enBookingStatus>()
                    {
                        enBookingStatus.Confirmed,
                        enBookingStatus.Cancelled
                    };

                case enBookingStatus.Confirmed:
                    return new List<enBookingStatus>()
                    {
                        enBookingStatus.Active,
                        enBookingStatus.Cancelled,
                        enBookingStatus.NoShow
                    };

                case enBookingStatus.Active:
                    return new List<enBookingStatus>()
                    {
                        enBookingStatus.Completed
                    };

                default: return new List<enBookingStatus>();
            }
        }
        public async Task<clsServiceResult<bool>> CanStartVehicleReturnAsync(int bookingID)
        {
            bool hasVehicleReturn = await clsRentalBookingData.HasVehicleReturnAsync(bookingID);

            if (hasVehicleReturn)
                return clsServiceResult<bool>.Fail("يوجد عملية إرجاع لهذا الحجز");


            return clsServiceResult<bool>.OK(true);
        }
        public async Task<clsServiceResult<bool>> IsDuplicateVehilceDamageAsync(int bookingID)
        {
            bool hasDuplicate = await clsVehicleDamageData.IsDuplicateBookingIDAsync(null,bookingID);

            if (hasDuplicate)
                return clsServiceResult<bool>.Fail("توجد عملية ضرر مسجلة لهذا الحجز");


            return clsServiceResult<bool>.OK(true);
        }


        public async Task<clsServiceResult<decimal>> GetRentalPricePerDayAsync(int vehicleId , DateTime startDate , int rentalDays)
        {
            decimal? rentalPriceDay = await clsRatePlansData.GetRentalPricePerDayAsync(vehicleId, startDate, rentalDays);
            if (!rentalPriceDay.HasValue)
                return clsServiceResult<decimal>.Fail("تعذر احتساب سعر الإيجار");

            return clsServiceResult<decimal>.OK(rentalPriceDay.Value);
        }
        public async Task<clsServiceResult<List<clsLocationComboBoxDto>>> GetAllLocationAsync()
        {
            var locations = await clsLocationsData.GetAllLocationsAsync();
            if (locations == null || locations.Count == 0)
                return clsServiceResult<List<clsLocationComboBoxDto>>.Fail("لاتوجد مواقع");

            var list = locations.Select(clsLocationMapper.ToComboBoxDto).ToList();

            return clsServiceResult<List<clsLocationComboBoxDto>>.OK(list);
        }


        // ===================== Reports ====================

        public  async Task<clsServiceResult<DataTable>> GetAllRentalsReportAsync(DateTime fromDate, DateTime toDate)
        {
            var result = await clsRentalBookingData.GetAllRentalsReportAsync(fromDate,toDate);
            if (result == null || result.Rows.Count == 0)
                return clsServiceResult<DataTable>.Fail("لاتوجد بيانات في الفترة المحددة");

            return clsServiceResult<DataTable>.OK(result);
        }
        public  async Task<clsServiceResult<DataTable>> GetRentalRevenueReportAsync(DateTime fromDate, DateTime toDate)
        {
            var result = await clsRentalBookingData.GetRentalRevenueReportAsync(fromDate, toDate);
            if (result == null || result.Rows.Count == 0)
                return clsServiceResult<DataTable>.Fail("لاتوجد بيانات في الفترة المحددة");

            return clsServiceResult<DataTable>.OK(result);
        }
        public  async Task<clsServiceResult<DataTable>> GetActiveRentalsReportAsync()
        {
            var result = await clsRentalBookingData.GetActiveRentalsReportAsync();
            if (result == null || result.Rows.Count == 0)
                return clsServiceResult<DataTable>.Fail("لاتوجد بيانات");

            return clsServiceResult<DataTable>.OK(result);
        }
    }
}
