using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Users;
using CarRental_Buisness.Results;
using CarRental_DataAccess;
using CarRental_Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness
{
    public class clsLoginService
    {
        private const int MaxFailedAttempts = 5;

        public static async Task<clsServiceResult<clsUserDto>> LoginAsync(string userName, string password)
        {
            var entity = await clsUsersData.GetUserByUserNameAsync(userName);

            if (entity == null)
                return clsServiceResult<clsUserDto>.Fail("اسم المستخدم غير صحيح");

            if (!entity.IsActive)
                return clsServiceResult<clsUserDto>.Fail("الحساب غير نشط");

            if (entity.IsDeleted)
                return clsServiceResult<clsUserDto>.Fail("الحساب محذوف");

            if (entity.IsLockedOut)
                return clsServiceResult<clsUserDto>.Fail("الحساب مغلق، راجع مدير النظام");

            if (!clsSecurity.Verfiy(password, entity.Password))
            {
                await RegisterFaildLoginAsync(entity);
                return clsServiceResult<clsUserDto>.Fail("كلمة المرور غير صحيحة");
            }

            await ResetFaildLoginAsync(entity);

            var dto = clsUserMapper.ToDto(entity);

            clsCurrentUser.Set(dto);

            //clsSQLHelper.CurrentContext = new clsDbSessionContext
            //{
            //    UserID = clsCurrentUser.UserID ,
            //    MachineName = Environment.MachineName ,
            //    IPAddress = "123" ,
            //    Source = "UI"
            //};

            return clsServiceResult<clsUserDto>.OK(dto);
        }
        private static async Task RegisterFaildLoginAsync(clsUsersEntities entity)
        {
            entity.FailedLoginAttempts++;
            entity.LastFailedLoginDate = DateTime.Now;

            if(entity.FailedLoginAttempts>=MaxFailedAttempts)
            {
                entity.IsLockedOut = true;
            }

            await clsUsersData.UpdateAsync(entity);
        }
        private static async Task ResetFaildLoginAsync(clsUsersEntities entity)
        {
            entity.FailedLoginAttempts = 0;
            entity.LastFailedLoginDate= null;
            entity.IsLockedOut = false;
            await clsUsersData.UpdateAsync(entity);
        }
    }
}
