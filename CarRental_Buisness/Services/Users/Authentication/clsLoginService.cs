using CarRental_Buisness.Helpers;
using CarRental_Buisness.Mappers;
using CarRental_Buisness.Models.Users;
using CarRental_Buisness.Results;
using CarRental_Buisness.Services.Permissions.RolePermission;
using CarRental_DataAccess;
using CarRental_Entities;
using SharedClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental_Buisness
{
    public class clsLoginService
    {
        private static  readonly clsRolePermissionService _rolePermissionService = new clsRolePermissionService();
        private const int MaxFailedAttempts = 5;

        public static async Task<clsServiceResult<clsUserDto>> LoginAsync(string userName, string password)
        {
            try
            {
                _InitializeContext();

                var userResult = await _GetValidUserAsync(userName);
                if (!userResult.Success)
                    return clsServiceResult<clsUserDto>.Fail(userResult.ErrorMessage);

                var entity = userResult.Data;

                var passwordResult =await _ValidatePasswordAsync(entity, password);

                if (!passwordResult.Success)
                    return clsServiceResult<clsUserDto>.Fail(passwordResult.ErrorMessage);

                var dto = clsUserMapper.ToDto(entity);

                var permissionResult = await _LoadPermissionsAsync(dto.RoleID);

                if (!permissionResult.Success)
                {
                    _ClearCurrentSession();
                    return clsServiceResult<clsUserDto>.Fail(permissionResult.ErrorMessage);
                }

                _InitializeUserSession(dto, permissionResult.Data);
                return clsServiceResult<clsUserDto>.OK(dto);
            }
            catch (Exception ex)
            {
                _ClearCurrentSession();
                clsEventLogger.LogException("clsLoginService.LoginAsync", ex);
                return clsServiceResult<clsUserDto>.Fail("حدث خطأ أثناء تسجيل الدخول");
            }
        }
        private static void _InitializeUserSession(clsUserDto user,IEnumerable<string> permissions)
        {
            clsCurrentUser.Set(user);
            clsAuthorizationCache.LoadPermissions(permissions);
            clsSQLHelper.CurrentContext.UserID = user.UserID;
        }
        private static void _ClearCurrentSession()
        {
            clsCurrentUser.Clear();
            clsAuthorizationCache.Clear();
            clsSQLHelper.CurrentContext = null;
        }
        private static async Task<clsServiceResult<IEnumerable<string>>> _LoadPermissionsAsync(int roleID)
        {
            var result =
                await _rolePermissionService.GetPermissionsForRoleAsync(roleID);

            if (!result.Success)
                return clsServiceResult<IEnumerable<string>>.Fail("تعذر تحميل صلاحيات المستخدم");

            var permissions =result.Data
                      .Where(p => p.IsAllowed)
                      .Select(p => p.PermissionCode);

            return clsServiceResult<IEnumerable<string>>.OK(permissions);
        }
        private static async Task<clsServiceResult<bool>> _ValidatePasswordAsync(clsUsersEntities entity,string password)
        {
            if (!clsSecurity.Verify(password, entity.Password))
            {
                await _RegisterFailedLoginAsync(entity.UserID);
                return clsServiceResult<bool>.Fail("اسم المستخدم أو كلمة المرور غير صحيحة");
            }

            await _ResetFailedLoginAsync(entity.UserID);
            return clsServiceResult<bool>.OK(true);
        }
        private static async Task<clsServiceResult<clsUsersEntities>> _GetValidUserAsync(string userName)
        {
            var entity = await clsUsersData.GetUserByUserNameAsync(userName);

            if (entity == null)
                return clsServiceResult<clsUsersEntities>.Fail("اسم المستخدم أو كلمة المرور غير صحيحة");

            if (!entity.IsActive)
                return clsServiceResult<clsUsersEntities>.Fail("الحساب غير نشط");

            if (entity.IsDeleted)
                return clsServiceResult<clsUsersEntities>.Fail("الحساب محذوف");

            if (entity.IsLockedOut)
                return clsServiceResult<clsUsersEntities>.Fail("الحساب مغلق، راجع مدير النظام");

            return clsServiceResult<clsUsersEntities>.OK(entity);
        }
        private static void _InitializeContext()
        {
            clsSQLHelper.CurrentContext = new clsDbSessionContext
            {
                UserID = null,
                MachineName = Environment.MachineName,
                IPAddress = clsUtil.GetLocalIPAddress(),
                Source = "WinForms"
            };
        }
        private static async Task _RegisterFailedLoginAsync(int userId)
        {
            await clsUsersData.RegisterFailedLoginAsync(userId, MaxFailedAttempts);
        }
        private static async Task _ResetFailedLoginAsync(int userId)
        {
            await clsUsersData.ResetFailedLoginAsync(userId);
        }
    }
}
