using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Models.RentalBooking;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Invoices;
using CarRental_Buisness.Services.RentalBooking;
using SharedClass;
using System;
using System.Threading.Tasks;
using System.Transactions;
namespace CarRental_Buisness.Services.WorkFlow
{
    public class clsRentalBookingWorkflowService
    {
        private readonly clsRentalBookingService _bookingService;
        private readonly clsInvoicesService _invoiceService;

        public clsRentalBookingWorkflowService(clsRentalBookingService rentalBookingService)
        {
            _bookingService = rentalBookingService;
            _invoiceService = new clsInvoicesService();
        }

        public async Task<clsServiceResult<clsRentalBookingDto>> CreateBookingAsync(clsRentalBookingAddNewModel bookingModel ,
            clsApplicationSettings settings ,DateTime driverLicenseExpiry)
        {
            using(var scope = new TransactionScope(TransactionScopeOption.Required ,
                                                  new TransactionOptions
                                                   {
                                                       IsolationLevel = IsolationLevel.ReadCommitted
                                                   } , TransactionScopeAsyncFlowOption.Enabled))
            {

                var bookingResult = await _bookingService.AddNewAsync(bookingModel, driverLicenseExpiry);

                if(!bookingResult.Success)
                {
                    return bookingResult;
                }

                var invoiceMode = _BuildBookingInvoice(bookingResult.Data, settings);

                var invoiceResult = await _invoiceService.AddNewAsync(invoiceMode);

                if (!invoiceResult.Success)
                {
                    if (!string.IsNullOrWhiteSpace(invoiceResult.ErrorMessage))
                        return clsServiceResult<clsRentalBookingDto>.Fail(invoiceResult.ErrorMessage);
                    else
                        return clsServiceResult<clsRentalBookingDto>.Invalid(invoiceResult.Validation);
                }

                scope.Complete();

                return bookingResult;
            }
        }
        public async Task<clsServiceResult<bool>> UpdateBookingAsync( int bookingID,clsRentalBookingUpdateModel bookingModel,
              clsApplicationSettings settings)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {

                var bookingResult = await _bookingService.UpdateAsync(bookingID, bookingModel);

                if (!bookingResult.Success)
                    return bookingResult;

                var booking = await _bookingService.GetRentalBookingByIDAsync(bookingID);

                if (!booking.Success)
                    return clsServiceResult<bool>.Fail("تعذر قراءة بيانات الحجز.");

                var invoiceModel = _BuildUpdateBookingInvoice(booking.Data, settings);

                var invoiceResult = await _invoiceService.UpdateLinkedInvoiceAsync(bookingID,invoiceModel);

                if (!invoiceResult.Success)
                {
                    if(!string.IsNullOrWhiteSpace(invoiceResult.ErrorMessage))
                        return clsServiceResult<bool>.Fail(invoiceResult.ErrorMessage);
                    else
                        return clsServiceResult<bool>.Invalid(invoiceResult.Validation);
                }
                   

                scope.Complete();

                return clsServiceResult<bool>.OK(true);
            }
        }
        private clsInvoicesAddNewModel _BuildBookingInvoice(  clsRentalBookingDto booking, clsApplicationSettings settings)
        {
            int rentalDays =
                (booking.RentalEndDate.Date - booking.RentalStartDate.Date).Days;

            decimal rentalAmount =
                booking.RentalPricePerDay * rentalDays;

            return new clsInvoicesAddNewModel
            {
                InvoiceTypeID = enInvoiceTypes.Booking,
                BookingID = booking.BookingID,
                BaseAmount = rentalAmount,
                AdditionalCharges = 0m,
                LateFees = 0m,
                TaxAmount = rentalAmount * settings.TaxRate,
                DiscountAmount = 0m,
                CurrencyCode = settings.CurrencyCode,
                Notes = "فاتورة الحجز"
            };
        }
        private clsUpdateLinkedInvoiceModel _BuildUpdateBookingInvoice(clsRentalBookingDto booking , clsApplicationSettings settings)
        {

            int rentalDays = (booking.RentalEndDate.Date - booking.RentalStartDate.Date).Days;

            decimal rentalAmount =booking.RentalPricePerDay * rentalDays;

            return new clsUpdateLinkedInvoiceModel
            {
                EntityID = booking.BookingID,
                InvoiceType = enInvoiceTypes.Booking,
                BaseAmount = rentalAmount,
                AdditionalCharges = 0m,
                LateFees = 0m,
                TaxAmount = rentalAmount * settings.TaxRate,
                DiscountAmount = 0m,
                CurrencyCode= settings.CurrencyCode,
                Notes = "تم تحديث الفاتورة بعد تعديل الحجز"
            };
        }
    }
}
