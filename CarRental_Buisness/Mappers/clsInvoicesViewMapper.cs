using CarRental_Buisness.Models.Invoices;
using CarRental_Entities.Invoices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsInvoicesViewMapper
    {
        public static clsInvoicesViewDto ToDto(clsInvoicesViewEntities entity)
        {
            return new clsInvoicesViewDto
            {
                InvoiceID = entity.InvoiceID,
                InvoiceNumber = entity.InvoiceNumber,
                TypeName = entity.TypeName,
                BookingID = entity.BookingID,
                MaintenanceID = entity.MaintenanceID,
                DamageID = entity.DamageID,
                InvoiceDate = entity.InvoiceDate,
                BaseAmount = entity.BaseAmount,
                AdditionalCharges = entity.AdditionalCharges,
                LateFees = entity.LateFees,
                TaxAmount = entity.TaxAmount,
                DiscountAmount = entity.DiscountAmount,
                TotalAmount = entity.TotalAmount,
                CurrencyCode = entity.CurrencyCode,
                Notes = entity.Notes,
                CreatedDate = entity.CreatedDate,
                CreatedByUserID = entity.CreatedByUserID,
                EditedDate = entity.EditedDate,
                EditedByUserID = entity.EditedByUserID,
                CustomerID = entity.CustomerID,
                VehicleID = entity.VehicleID
            };
        }
    }
}
