using CarRental_Buisness.Helpers;
using CarRental_Buisness.Models.Invoices;
using CarRental_Buisness.Models.RentalBooking;
using CarRental_Buisness.Models.VehicleReturn;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Invoices;
using CarRental_Buisness.Services.RentalBooking;
using CarRental_Buisness.Services.VehicleReturn;
using System.Threading.Tasks;
using System.Transactions;

namespace CarRental_Buisness.Services.WorkFlow
{
    public class clsVehicleReturnWorkflowService
    {
        private readonly clsRentalBookingService _bookingService;
        private readonly clsVehicleReturnService _vehicleReturnService;
        private readonly clsInvoicesService _invoiceService;

        public clsVehicleReturnWorkflowService(clsVehicleReturnService vehicleReturnService )
        {
            _bookingService = new clsRentalBookingService();
            _vehicleReturnService = vehicleReturnService;
            _invoiceService = new clsInvoicesService();
        }

        public async Task<clsServiceResult<bool>> MarkAsInvoicedAsync( int bookingID, int returnId, int lateDays,
            decimal additionalCharges,clsApplicationSettings settings)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted
                },
                TransactionScopeAsyncFlowOption.Enabled))
            {

                var bookingResult = await _bookingService.GetRentalBookingByIDAsync(bookingID);

                if (!bookingResult.Success)
                    return clsServiceResult<bool>.Fail(bookingResult.ErrorMessage);

                var invoiceModel =_BuildUpdateInvoice(bookingResult.Data , lateDays , additionalCharges,settings);

                var invoiceResult = await _invoiceService.UpdateLinkedInvoiceAsync(bookingID,invoiceModel);
                if (!invoiceResult.Success)
                {
                    if (!string.IsNullOrWhiteSpace(invoiceResult.ErrorMessage))
                        return clsServiceResult<bool>.Fail(invoiceResult.ErrorMessage);
                    else
                        return clsServiceResult<bool>.Invalid(invoiceResult.Validation);
                }

                var markResult =  await _vehicleReturnService.MarkAsInvoicedAsync(returnId);

                if (!markResult.Success)
                    return markResult;


                scope.Complete();
                return clsServiceResult<bool>.OK(true);
            }
        }

        private clsUpdateLinkedInvoiceModel _BuildUpdateInvoice( clsRentalBookingDto booking,int lateDays , decimal? AdditionalCharges, clsApplicationSettings settings)
        {
            int rentalDays =(booking.RentalEndDate.Date - booking.RentalStartDate.Date).Days;

            decimal rentalAmount = booking.RentalPricePerDay * rentalDays;
            decimal lateFees =lateDays * booking.RentalPricePerDay;
            decimal additionalCharges = AdditionalCharges ?? 0;
            decimal tax =(rentalAmount + lateFees + additionalCharges) * settings.TaxRate;

            return new clsUpdateLinkedInvoiceModel
            {
                EntityID = booking.BookingID,
                InvoiceType = SharedClass.enInvoiceTypes.Booking,
                BaseAmount = rentalAmount,
                AdditionalCharges = additionalCharges,
                LateFees = lateFees,
                TaxAmount = tax,
                DiscountAmount = 0,
                CurrencyCode = settings.CurrencyCode,
                Notes = "تم تحديث الفاتورة بعد إرجاع المركبة"
            };
        }
    }
}