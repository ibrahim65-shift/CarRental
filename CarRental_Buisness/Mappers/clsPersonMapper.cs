using CarRental_Buisness.Models.People;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsPersonMapper
    {
        public static clsPersonDto ToDto(clsPersonEntities entity)
        {
            return new clsPersonDto
            {
                PersonID = entity.PersonID,
                NationalNo = entity.NationalNo,
                FirstName = entity.FirstName,
                SecondName = entity.SecondName,
                ThirdName = entity.ThirdName,
                LastName = entity.LastName,
                BirthDate = entity.BirthDate,
                Gender = entity.Gender,
                Email = entity.Email,
                Phone = entity.Phone,
                Address = entity.Address,
            };
        }
    }
}
