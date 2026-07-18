using CarRental_Buisness.Models.Users;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsUserMapper
    {
        public static clsUserDto ToDto(clsUsersEntities entity)
        {
            return new clsUserDto
            { 
                UserID = entity.UserID,
                RoleID = entity.RoleID,
                UserName = entity.UserName,
                IsActive = entity.IsActive,
                IsLockedOut = entity.IsLockedOut,
                PersonID = entity.PersonID,
                PersonInfo = clsPersonMapper.ToDto(entity.Person),
            };

        }
    }
}
