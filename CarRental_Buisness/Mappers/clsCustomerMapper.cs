using CarRental_Buisness.Models.Customers;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsCustomerMapper
    {
        public static clsCustomerDto ToDto(clsCustomersEntities entity)
        {
            return new clsCustomerDto
            {
                CustomerID = entity.CustomerID,
                DriverLicenseNumber = entity.DriverLicenseNumber,
                DriverLicenseExpiry = entity.DriverLicenseExpiry,
                PersonID = entity.PersonID,

                PersonInfo = clsPersonMapper.ToDto(entity.Person),
            };
        }
    }
}
