using CarRental_Buisness.Models.PaymentMethods;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsPaymentMethodsMapper
    {
        public static clsPaymentMethodsDto ToDto(clsPaymentMethodsEntities entity)
        {
            return new clsPaymentMethodsDto
            { 
               PaymentMethodID = entity.PaymentMethodID,
               MethodName = entity.MethodName,
            };

        }
    }
}
