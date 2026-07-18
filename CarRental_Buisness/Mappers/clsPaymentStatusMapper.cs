using CarRental_Buisness.Models.PaymentStatus;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsPaymentStatusMapper
    {
        public static clsPaymentStatusDto ToDto(clsPaymentStatusEntities entity)
        {
            return new clsPaymentStatusDto
            { 
              PaymentStatusID = entity.PaymentStatusID,
              StatusName = entity.StatusName
            };

        }
    }
}
