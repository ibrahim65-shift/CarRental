using CarRental_Buisness.Models.ReturnStatus;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness.Mappers
{
    public class clsReturnStatusMapper
    {
        public static clsReturnStatusDto ToDto(clsReturnStatusEntities  entity)
        {
            return new clsReturnStatusDto
            { 
               ReturnStatusID = entity.ReturnStatusID,
               StatusName = entity.StatusName
            };

        }
    }
}
