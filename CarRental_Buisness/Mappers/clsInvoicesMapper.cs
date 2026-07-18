using CarRental_Buisness.Models.Invoices;
using CarRental_Entities.Invoices;


namespace CarRental_Buisness.Mappers
{
    public static class clsInvoicesMapper
    {
        public static clsInvoicesDto ToDto(clsInvoiceEntities entity)
        {
            return new clsInvoicesDto
            {
                InvoiceID = entity.InvoiceID,
                InvoiceNumber = entity.InvoiceNumber,
                InvoiceTypeID = entity.InvoiceTypeID,
                BookingID = entity.BookingID,
                MaintenanceID = entity.MaintenanceID,
                InvoiceDate = entity.InvoiceDate,
                BaseAmount = entity.BaseAmount,
                AdditionalCharges = entity.AdditionalCharges,
                LateFees = entity.LateFees,
                TaxAmount = entity.TaxAmount,
                DiscountAmount = entity.DiscountAmount,
                TotalAmount = entity.TotalAmount,
                CurrencyCode = entity.CurrencyCode,
                Notes = entity.Notes

            };
        }
    }
}
