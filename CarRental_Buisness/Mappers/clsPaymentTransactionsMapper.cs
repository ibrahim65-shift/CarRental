using CarRental_Buisness.Models.PaymentTransactions;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsPaymentTransactionsMapper
    {
        public static clsPaymentTransactionsDto ToDto(clsPaymentTransactionsEntities entity)
        {
            return new clsPaymentTransactionsDto
            {
                InvoiceID = entity.InvoiceID,
                PaymentID = entity.PaymentID,
                PaymentMethodID = entity.PaymentMethodID,
                PaymentStatusID = entity.PaymentStatusID,
                PaymentDate = entity.PaymentDate,
                PaidAmount = entity.PaidAmount,
                Reference = entity.Reference,
            };
        }
    }
}
